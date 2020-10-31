using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// INI文件读写类。 Copyright (C) Maticsoft
/// </summary>
public class INIFile
{
    #region Public Fields

    public string path;

    #endregion Public Fields

    #region Public Constructors

    public INIFile(string INIPath)
    {
        path = INIPath;
    }

    #endregion Public Constructors

    #region Public Methods

    /// <summary>
    /// 删除ini文件下所有段落
    /// </summary>
    public void ClearAllSection()
    {
        IniWriteValue(null, null, null);
    }

    /// <summary>
    /// 删除ini文件下personal段落下的所有键
    /// </summary>
    /// <param name="Section"> </param>
    public void ClearSection(string Section)
    {
        IniWriteValue(Section, null, null);
    }

    /// <summary>
    /// 读取INI文件
    /// </summary>
    /// <param name="Section"> </param>
    /// <param name="Key"> </param>
    /// <returns> </returns>
    public string IniReadValue(string Section, string Key)
    {
        var temp = new StringBuilder(255);
        var i = GetPrivateProfileString(Section, Key, "", temp, 255, path);
        return temp.ToString();
    }

    public byte[] IniReadValues(string section, string key)
    {
        var temp = new byte[255];
        var i = GetPrivateProfileString(section, key, "", temp, 255, path);
        return temp;
    }

    /// <summary>
    /// 写INI文件
    /// </summary>
    /// <param name="Section"> </param>
    /// <param name="Key"> </param>
    /// <param name="Value"> </param>
    public void IniWriteValue(string Section, string Key, string Value)
    {
        WritePrivateProfileString(Section, Key, Value, path);
    }

    #endregion Public Methods

    #region Private Methods

    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal,
        int size, string filePath);

    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section, string key, string defVal, byte[] retVal,
        int size, string filePath);

    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

    #endregion Private Methods
}