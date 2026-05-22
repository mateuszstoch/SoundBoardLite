namespace SoundBoardLite.Models;

public class SoundItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public double Volume { get; set; } = 1.0;
    public string Shortcut { get; set; } = string.Empty;
}
