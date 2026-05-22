using NAudio.Wave;
using System;
using System.Collections.Generic;

namespace SoundBoardLite.Services;

public class AudioDevice
{
    public int DeviceNumber { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class AudioPlayerService
{
    private WaveOutEvent? _waveOut;
    private AudioFileReader? _audioFile;

    public List<AudioDevice> GetOutputDevices()
    {
        var devices = new List<AudioDevice>();
        // Urządzenie domyślne (zazwyczaj -1 w NAudio)
        devices.Add(new AudioDevice { DeviceNumber = -1, Name = "Domyślne urządzenie systemu" });

        for (int i = 0; i < WaveOut.DeviceCount; i++)
        {
            var capabilities = WaveOut.GetCapabilities(i);
            devices.Add(new AudioDevice { DeviceNumber = i, Name = capabilities.ProductName });
        }

        return devices;
    }

    public void PlaySound(string filePath, double volume, int deviceNumber)
    {
        Stop();

        try
        {
            _audioFile = new AudioFileReader(filePath)
            {
                Volume = (float)volume
            };

            _waveOut = new WaveOutEvent
            {
                DeviceNumber = deviceNumber
            };

            _waveOut.Init(_audioFile);
            _waveOut.Play();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd odtwarzania: {ex.Message}");
        }
    }

    public void Stop()
    {
        _waveOut?.Stop();
        _waveOut?.Dispose();
        _waveOut = null;

        _audioFile?.Dispose();
        _audioFile = null;
    }
}
