using Common;
using System.Text;

string path = @"C:\Users\zjk\Desktop\temp.txt";
int pageNumDiff = 8;

string[] srcLines = IOUtil.GetFileTextArr(new FileInfo(path));
StringBuilder output = new();

#region 标题和页号不同行时临时打开
//for (int iLine = 0; iLine < srcLines.Length; iLine++)
//{
//    #region vscode不能一直打开看
//    //StreamWriter fileWriter = new StreamWriter(File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite));
//    //if ((iLine & 1) == 0)
//    //{
//    //    fileWriter.Write($"{srcLines[iLine]} ");
//    //}
//    //else
//    //{
//    //    fileWriter.WriteLine($"{srcLines[iLine]}");
//    //} 
//    #endregion
//    if ((iLine & 1) == 0)
//    {
//        output.Append($"{srcLines[iLine]} ");
//    }
//    else
//    {
//        output.AppendLine($"{srcLines[iLine]}");
//    }
//}
//IOUtil.CreateTextFile(path, output.ToString().Trim());
//output.Clear();
#endregion

for (int iLine = 0; iLine < srcLines.Length; iLine++)
{
    string line = srcLines[iLine].Trim();
    if (line.Length == 0) continue;
    int level = GetLevel(srcLines[iLine]);
    string[] info = line.Split(new char[] { ' ', '\t' });
    int pageNum = 0;
    try
    {
        pageNum = int.Parse(info.Last()) + pageNumDiff;
    }
    catch (Exception e)
    {
        Console.WriteLine($"txt第{iLine + 1}行：{e}");
        Console.ReadLine();
    }
    StringBuilder outputLine = new();
    for (int i = 0; i < info.Length - 1; i++)
    {
        outputLine.Append($"{info[i]} ");
    }
    string temp = outputLine.ToString();
    outputLine.Clear();
    for (int i = 0; i < level; i++)
    {
        outputLine.Append('\t');
    }
    outputLine.Append(temp.Trim());
    outputLine.Append($"\t{pageNum}");
    output.AppendLine(outputLine.ToString());
}

IOUtil.CreateTextFile(path, output.ToString().Trim());




static int GetLevel(string line)
{
    int level = 0;
    bool isTab = line.StartsWith('\t');
    for (int i = 0; i < line.Length; i++)
    {
        if (isTab ? line[i] == '\t' : line[i] == ' ')
        {
            level++;
        }
        else
        {
            break;
        }
    }
    return isTab ? level : level / 4;
}