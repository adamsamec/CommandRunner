﻿namespace CommandRunner
{ 
public class ConfigJson
{
    public Settings settings { get; set; }
    public History history { get; set; }
}

public class Settings
{
    public string findTextIgnoreCase { get; set; }
    public string checkForUpdateOnLaunch { get; set; }
    public string playSuccessSound { get; set; }
    public string successRegex { get; set; }
    public string playErrorSound { get; set; }
    public string errorRegex { get; set; }
}

public class History
{
    public string[] commands { get; set; }
    public string[] workingDirs { get; set; }
    public string[] findTexts { get; set; }
}
}
