using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FlagHandler : Singleton<FlagHandler>
{
    Dictionary<string, FlagBase> flags = new Dictionary<string, FlagBase>();

    private const string FILE_NAME = @"wo'ah.txt";
    private string path = string.Empty;
    
    void Start()
    {
        path = Application.persistentDataPath + "/" + FILE_NAME;
        CreateFile();
        ReadFile();
    }

    public void WriteFile()
    {
        CreateFile();
        List<string> lines = new List<string>();
        foreach(FlagBase flagBase in flags.Values)
        {
            if (!flagBase.writable) continue;
            string flag = flagBase.flag;
            if (flagBase.GetType() == typeof(IntFlag)) lines.Add(string.Concat(flag, ":INT:", ((IntFlag)flagBase).value));
            else if (flagBase.GetType() == typeof(FloatFlag)) lines.Add(string.Concat(flag, ":FLOAT:", ((FloatFlag)flagBase).value));
            else if (flagBase.GetType() == typeof(StringFlag)) lines.Add(string.Concat(flag, ":STRING:", ((StringFlag)flagBase).value));
            else if (flagBase.GetType() == typeof(BoolFlag)) lines.Add(string.Concat(flag, ":BOOL:", ((BoolFlag)flagBase).value));
        }

        try
        {
            File.WriteAllLines(path, lines);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    void CreateFile()
    {
        try
        {
            if (!File.Exists(path))
            {
                //Directory.CreateDirectory(path);
                File.Create(path).Close();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void ReadFile()
    {
        string[] lines = File.ReadAllLines(path);
        for (int i = 0; i < lines.Length; i++)
        {
            try
            {
                FlagBase flagBase = HandleLine(lines[i]);
                if (flagBase == null) continue;
                if (!flags.ContainsKey(flagBase.flag))
                    flags[flagBase.flag] = flagBase;
                else
                    flags.Add(flagBase.flag, flagBase);
            }
            catch(Exception e) { Debug.Log(e.Message); }
        }
    }

    List<string> properTypes = new List<string>() { "INT", "BOOL", "FLOAT", "STRING" };

    FlagBase HandleLine(string line)
    {
        string[] splitLine = line.Split(':');
        if (splitLine.Length != 3) return null;
        string flag = HandleAsFlag(splitLine[0]);
        string type = HandleAsType(splitLine[1]);
        string value = splitLine[2];
        if (!properTypes.Contains(type)) return null;
        FlagBase flagBase = null;
        if (type == "INT") flagBase = new IntFlag(flag, value);
        else if (type == "BOOL") flagBase = new BoolFlag(flag, value);
        else if (type == "FLOAT") flagBase = new FloatFlag(flag, value);
        else if (type == "STRING") flagBase = new StringFlag(flag, value);
        return flagBase;
    }

  

    public float? GetFlagFloat(string flag)
    {
        flag = SanitzeFlag(flag);
        if (!flags.ContainsKey(flag)) return null;
        FlagBase flagBase = flags[flag];
        if (flagBase.GetType() != typeof(FloatFlag)) return null;
        return ((FloatFlag)flagBase).value;
    }

    public int? GetFlagInt(string flag)
    {
        flag = SanitzeFlag(flag);
        if (!flags.ContainsKey(flag)) return null;
        FlagBase flagBase = flags[flag];
        if (flagBase.GetType() != typeof(IntFlag)) return null;
        return ((IntFlag)flagBase).value;
    }

    public string GetFlagString(string flag)
    {
        flag = SanitzeFlag(flag);
        if (!flags.ContainsKey(flag)) return string.Empty;
        FlagBase flagBase = flags[flag];
        if (flagBase.GetType() != typeof(StringFlag)) return string.Empty;
        return ((StringFlag)flagBase).value;
    }

    public bool? GetFlagBool(string flag)
    {
        flag = SanitzeFlag(flag);
        if (!flags.ContainsKey(flag)) return null;
        FlagBase flagBase = flags[flag];
        if (flagBase.GetType() != typeof(BoolFlag)) return null;
        return ((BoolFlag)flagBase).value;
    }
    
    public void SetFlag(string flag, float value, bool writable)
    {
        flag = SanitzeFlag(flag);
        if (!flags.ContainsKey(flag)) flags.Add(flag, new FloatFlag(flag, value.ToString(), writable));
        else ((FloatFlag)flags[flag]).value = value;
    }

    public void SetFlag(string flag, int value, bool writable)
    {
        flag = SanitzeFlag(flag);
        if (!flags.ContainsKey(flag)) flags.Add(flag, new IntFlag(flag, value.ToString(), writable));
        else ((IntFlag)flags[flag]).value = value;
    }

    public void SetFlag(string flag, bool value, bool writable)
    {
        flag = SanitzeFlag(flag);
        if (!flags.ContainsKey(flag)) flags.Add(flag, new BoolFlag(flag, value.ToString(), writable));
        else ((BoolFlag)flags[flag]).value = value;
    }

    public void SetFlag(string flag, string value, bool writable)
    {
        flag = SanitzeFlag(flag);
        if (!flags.ContainsKey(flag)) flags.Add(flag, new StringFlag(flag, value, writable));
        else ((StringFlag)flags[flag]).value = value;
    }

    string SanitzeFlag(string flag)
    {
        return flag.Replace(":", "_");
    }

    string HandleAsFlag(string line)
    {
        return SanitzeFlag(line);
    }
    string HandleAsType(string line)
    {
        return line.ToUpper();
    }

}

public class FlagBase
{
    public string flag;
    public bool writable;

    public FlagBase(string flag, bool writable)
    {
        this.flag = flag;
        this.writable = writable;
    }
}

public class FloatFlag : FlagBase
{
    public float value;
    public FloatFlag(string flag, string value, bool writable = true) : base(flag, writable)
    {
        this.value = float.Parse(value);
    }
}

public class IntFlag : FlagBase
{
    public int value;
    public IntFlag(string flag, string value, bool writable = true) : base(flag, writable)
    {
        this.value = int.Parse(value);
    }
}

public class BoolFlag : FlagBase
{
    public bool value;
    public BoolFlag(string flag, string value, bool writable = true) : base(flag, writable)
    {
        this.value = bool.Parse(value);
    }
}

public class StringFlag : FlagBase
{
    public string value;
    public StringFlag(string flag, string value, bool writable = true) : base(flag, writable)
    {
        this.value = value;
    }
}