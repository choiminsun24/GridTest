using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CSVReader : MonoBehaviour
{
    //정규식
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\n";
    static char[] TRIM_CHARS = { '\"' };

    public static Dictionary<int, Dictionary<string, string>> Read(string path, int key)
    {
        Dictionary<int, Dictionary<string, string>> list = new Dictionary<int, Dictionary<string, string>>();
        TextAsset data = Resources.Load(path) as TextAsset;

        //행으로 분해
        string[] lines = Regex.Split(data.text, LINE_SPLIT_RE);
        if (lines.Length <= 1) return list; //내용이 없거나 헤더만 있으면

        //header 처리
        string[] header = Regex.Split(lines[0], SPLIT_RE);
        
        //각 행 정리
        for (int i = 1; i < lines.Length; i++)
        {
            int k = -1;

            //열로 나누고
            string[] line = Regex.Split(lines[i], SPLIT_RE);
            if (line.Length == 0 || line[0] == "") continue;

            //Dictionary에 저장
            Dictionary<string, string> column = new Dictionary<string, string>();

            for (int j = 0; j < line.Length; j++)
            {
                string value = line[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS);
                if (j == key)
                    k = int.Parse(value);
                else
                    column[header[j]] = value;
            }
            if (k == -1)
                Debug.Log("오류입니다. id가 없습니다.");
            else
                list[k] = column;
        }
        
        return list;
    }

}
