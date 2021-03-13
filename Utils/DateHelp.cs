using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2020_backend.Utils
{
    public class DateHelp
    {
        public static string GetDay(int day)
        {
            switch (day)
            {
                case 1:
                    return "24日";
                case 2:
                    return "25日";
                case 3:
                    return "26日";
                case 4:
                    return "27日";
                case 5:
                    return "28日";
                case 6:
                    return "29日";
                case 7:
                    return "30日";
                default:
                    return "暂无";
            }
        }
        public static string GetGrade(int grade)
        {
            switch (grade)
            {
                case 1:
                    return "大一";
                case 2:
                    return  "大二";
                case 3:
                    return "大三";
                case 4:
                    return "大四";
                default:
                    return "暂缺";
                    

            }
        }
        public static string GetEtc(int etc)
        {
            switch (etc)
            {
                case 1:
                    return "第一场";
                case 2:
                    return "第二场";
                case 3:
                    return "第三场";
                case 4:
                    return "第四场";
                case 5:
                    return "第五场";
                default:
                    return "暂无";
            }
        }
        public static string GetDepartment(int num)
        {
            switch (num)
            {
                case 0:
                    return "暂缺";
                case -1:
                    return "白给了";
                case 1:
                    return "电脑部";
                case 2:
                    return "电器部";
                case 3:
                    return "文宣部";
                case 4:
                    return "人资部";
                case 5:
                    return "财外部";
                default:
                    return String.Empty;
            }
        }
        public static string GetFirstReason(int num)
        {
            switch (num)
            {
                case 0:
                    return "暂缺";
                case -1:
                    return "错误";
                case 1:
                    return "电脑部：说说你跟电脑有关的印象深刻的事情（或者你对电脑最感兴趣的地方，可以是某个组成部分）";
                case 2:
                    return "电器部：你现在有哪些电子或技术方面的知识或经历吗？若有，请简述；若无，请讲讲你当下最想学会的一项技能，并打算如何运用它";
                case 3:
                    return "文宣部：在文宣方面，你有什么优势或者打算如何提高自己？";
                case 4:
                    return "人资部：请举一个你想出的创新点子（实现过或现实中有一定的可操作性），最好是关于组织某次活动（活动包括但不限于宣传活动，内建活动，特殊的生日祝福等等）";
                case 5:
                    return "财外部：在社团全员大会/部门内建/部门事务这三种情况打破了你原来的学习安排的情况下，你如何去平衡社团与学习生活？";
                default:
                    return String.Empty;
            }
        }
        public static string GetSecondReason(int num)
        {
            switch (num)
            {
                case 0:
                    return "暂缺";
                case -1:
                    return "错误";
                case 1:
                    return "电脑部：来电脑部之后想学些什么?有什么想学的也可以说呀";
                case 2:
                    return "电器部：你认为动手能力和理论知识对于电器维修来说哪个更重要？为什么？";
                case 3:
                    return "文宣部：描述一下你构想的以xx为主题的海报or摄影作品，xx可以是你想到的任何事物";
                case 4:
                    return "人资部：你觉得你身上最符合人资部的特点是什么？";
                case 5:
                    return "财外部：说说令你印象最深刻的理财经历，可以是任何和理财有关的哦";
                default:
                    return String.Empty;
            }
        }
        public static string GetThirdReason(int num)
        {
            switch (num)
            {
                case 0:
                    return "暂缺";
                case -1:
                    return "错误";
                case 1:
                    return "电脑部：说说你跟电脑有关的印象深刻的事情（或者你对电脑最感兴趣的地方，可以是某个组成部分）";
                case 2:
                    return "电器部：你有没有自己拆装简单电器的经历，有的话请简述";
                case 3:
                    return "文宣部：在文宣方面，你有什么优势或者打算如何提高自己？";
                case 4:
                    return "人资部：请举一个你想出的创新点子（实现过或现实中有一定的可操作性），最好是关于组织某次活动（活动包括但不限于宣传活动，内建活动，特殊的生日祝福等等）";
                case 5:
                    return "财外部：在社团全员大会/部门内建/部门事务这三种情况打破了你原来的学习安排的情况下，你如何去平衡社团与学习生活？";
                default:
                    return String.Empty;
            }
        }
    }
}
