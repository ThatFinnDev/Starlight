using Il2CppMonomiPark.SlimeRancher.Audio;
using Starlight.Enums.Sounds;

namespace Starlight.Utils;

public static class AudioEUtil
{
    internal static readonly Dictionary<MenuSound, SECTR_AudioCue> _menuSounds = new Dictionary<MenuSound, SECTR_AudioCue>();

    public static Dictionary<MenuSound, SECTR_AudioCue> menuSounds => _menuSounds;
    /*
    internal static UIAudioTable _defaultMenuSounds;
    public static UIAudioTable defaultMenuSounds => _defaultMenuSounds;
    
    public static SECTR_AudioCue GetMenuSound(string soundName)
    {
        if (_defaultMenuSounds == null) return null;
        if (_defaultMenuSounds._audioCueDictionary.ContainsKey(soundName))
            return _defaultMenuSounds._audioCueDictionary[soundName];
        return null;
    }
    public static SECTR_AudioCueInstance PlayMenuSound(string soundName) => PlaySound(GetMenuSound(soundName));*/
    
    
    public static SECTR_AudioCueInstance PlaySound(SECTR_AudioCue cue)
    {
        if(cue==null) return null;
        return AudioUtil.PlayCue(cue);
    } 
    public static SECTR_AudioCueInstance PlaySound(MenuSound sound)
    {
        if (!_menuSounds.ContainsKey(sound)) return null;
        var cue = _menuSounds[sound];
        if (cue == null) return null;
        return PlaySound(cue);
    }
}