using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ASP.NET_FinalTermExam.Models
{
    public class EMP
    {
        /// <summary>
        /// 建構式
        /// </summary>
        public EMP()
        {
           
        }

        /// <summary>
        /// 編號
        /// </summary>        
        [DisplayName("編號")]
        [Required()]
        public int Id {get;set;}

        /// <summary>
        /// 姓名
        /// </summary>
        [DisplayName("姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 職稱
        /// </summary>
        [DisplayName("職稱")]
        public string Title { get; set; }

        /// <summary>
        /// 任職日期
        /// </summary>
        /// 
        [DisplayName("任職日期")]
        public DateTime ? Hiredate {get;set;}

        /// <summary>
        /// 性別
        /// </summary>
        [DisplayName("性別")]
        public string Genter { get; set; }

        /// <summary>
        /// 年齡
        /// </summary>
        [DisplayName("年齡")]
        public int Age { get; set; }
    }
}
