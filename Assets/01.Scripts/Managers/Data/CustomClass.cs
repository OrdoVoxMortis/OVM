using System;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheet.Type;

namespace Hamster.ZG.Type
{
    public class CombineRule
    {
        public CombineType RuleType;
        public BlockType AllowedType; // 유형 허용
        public List<int> AllowedBlocksIds = new(); // 특정 블럭 허용
    }

    [Type(typeof(CombineRule), new string[] {"CombineRule"})]
    public class CombineRuleType : IType
    {
        public object DefaultValue => null;
        /// <summary>
        /// value는 스프레드 시트에 적혀있는 값
        /// </summary> 

        public object Read(string value)
        {
            string[] split = value.Split(',');
            List<int> idList = new List<int>();
            for (int i = 2; i < split.Length; i++)
            {
                split[i] = split[i].Replace("[", string.Empty).Replace("]", string.Empty);


                if (int.TryParse(split[i].Trim(), out int id))
                {
                    idList.Add(id);
                }
                
            }

            return new CombineRule()
            {
                RuleType = (CombineType)Enum.Parse(typeof(CombineType), split[0]),
                AllowedType = (BlockType)Enum.Parse(typeof(BlockType), split[1]),
                AllowedBlocksIds = idList

            };
        }

        /// <summary>
        /// value write to google sheet
        /// </summary> 
        public string Write(object value)
        {
            return null;
        }
    }

    [Type(typeof(bool), new string[] { "bool", "Bool" })]
    public class BoolType : IType
    {
        public object DefaultValue => false;

        public object Read(string value)
        {
            return value.Trim().ToLower() switch
            {
                "TRUE" => true,
                "true" => true,
                "FALSE" => false,
                "false" => false,
                "1" => true,
                "0" => false,
                _ => throw new FormatException($"BoolType: Invalid bool value '{value}'")
            };
        }

        public string Write(object value)
        {
            return null;
        }
    }
}

