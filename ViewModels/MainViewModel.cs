using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SoundBoardLite.Database;
using SoundBoardLite.Models;
using SoundBoardLite.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SoundBoardLite.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly DatabaseHelper _db;
    private readonly AudioPlayerService _audio;

    [ObservableProperty]
    private ObservableCollection<SoundItem> _sounds = new();

    [ObservableProperty]
    private ObservableCollection<AudioDevice> _audioDevices = new();

    [ObservableProperty]
    private AudioDevice? _selectedAudioDevice;

    public MainViewModel()
    {
        _db = new DatabaseHelper();
        _audio = new AudioPlayerService();
    }

    public async Task InitializeAsync()
    {
        await _db.InitializeAsync();
        
        var dbSounds = await _db.GetSoundsAsync();
        Sounds = new ObservableCollection<SoundItem>(dbSounds);

        var devices = _audio.GetOutputDevices();
        AudioDevices = new ObservableCollection<AudioDevice>(devices);
        
        var savedDeviceIdStr = await _db.GetSettingAsync("OutputDeviceId");
        if (int.TryParse(savedDeviceIdStr, out int savedDeviceId))
        {
            var device = AudioDevices.FirstOrDefault(d => d.DeviceNumber == savedDeviceId);
            SelectedAudioDevice = device ?? AudioDevices.FirstOrDefault();
        }
        else
        {
            SelectedAudioDevice = AudioDevices.FirstOrDefault();
        }
        
        RegisterAllHotkeys();
    }

    [RelayCommand]
    private async Task PlaySound(SoundItem sound)
    {
        if (sound == null || SelectedAudioDevice == null) return;
        
        // Zapisujemy ewentualne zmiany głośności do bazy przed odtworzeniem
        await _db.UpdateSoundAsync(sound);
        
        _audio.PlaySound(sound.FilePath, sound.Volume, SelectedAudioDevice.DeviceNumber);
    }
    
    [RelayCommand]
    private async Task AddSoundAsync()
    {
        var openFileDialog = new Microsoft.Win32.OpenFileDialog
        {
            Filter = "Pliki audio (*.mp3;*.wav)|*.mp3;*.wav|Wszystkie pliki (*.*)|*.*",
            Title = "Wybierz plik dźwiękowy"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            var fileName = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);
            var newSound = new SoundItem
            {
                Name = fileName,
                FilePath = openFileDialog.FileName,
                Volume = 1.0,
                Shortcut = "" 
            };

            await _db.AddSoundAsync(newSound);
            
            var dbSounds = await _db.GetSoundsAsync();
            Sounds = new ObservableCollection<SoundItem>(dbSounds);
        }
    }

    [RelayCommand]
    private async Task SaveSoundAsync(SoundItem sound)
    {
        if (sound == null) return;
        await _db.UpdateSoundAsync(sound);
        RegisterHotkey(sound);
    }

    public void RegisterAllHotkeys()
    {
        foreach (var sound in Sounds)
        {
            RegisterHotkey(sound);
        }
    }

    private void RegisterHotkey(SoundItem sound)
    {
        if (string.IsNullOrWhiteSpace(sound.Shortcut)) return;
        
        try
        {
            var parts = sound.Shortcut.Split('+');
            System.Windows.Input.ModifierKeys modifiers = System.Windows.Input.ModifierKeys.None;
            System.Windows.Input.Key key = System.Windows.Input.Key.None;
            
            foreach (var part in parts)
            {
                var p = part.Trim().ToUpper();
                if (p == "CTRL" || p == "CONTROL") modifiers |= System.Windows.Input.ModifierKeys.Control;
                else if (p == "ALT") modifiers |= System.Windows.Input.ModifierKeys.Alt;
                else if (p == "SHIFT") modifiers |= System.Windows.Input.ModifierKeys.Shift;
                else System.Enum.TryParse(p, true, out key);
            }
            
            if (key != System.Windows.Input.Key.None)
            {
                NHotkey.Wpf.HotkeyManager.Current.AddOrReplace(sound.Id.ToString(), key, modifiers, async (s, e) => 
                {
                    await PlaySound(sound);
                });
            }
        }
        catch { }
    }

    partial void OnSelectedAudioDeviceChanged(AudioDevice? value)
    {
        if (value != null)
        {
            _ = _db.SetSettingAsync("OutputDeviceId", value.DeviceNumber.ToString());
        }
    }
}
