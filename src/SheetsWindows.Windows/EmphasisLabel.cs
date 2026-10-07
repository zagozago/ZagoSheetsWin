namespace SheetsWindows.Windows;

// Native Label provides script shaping, bidirectional text and Windows font fallback.
// Historical phrase keys stay in the catalog; sentence emphasis is disabled.
internal sealed class EmphasisLabel : Label
{
    public EmphasisLabel() { UseMnemonic = false; AutoEllipsis = false; }
    internal bool HasEmphasis => Font.Bold;
}
