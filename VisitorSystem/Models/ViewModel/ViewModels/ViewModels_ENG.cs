using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace VisitorSystem.Models
{
    /// <summary>
    /// 내방객 정보 모두가 담겨있는 비즈니스 모델
    /// </summary>
    public class VisitorInfoEng
    {

        [Display(Name = "내방객 ID")]
        public int VisitorID { get; set; }

        [Required]
        [Display(Name = "성명")]
        public string VisitorName { get; set; }

        [Required]
        [Display(Name = "연락처")]
        public string VisitorMobile { get; set; }

        [Required]
        [Display(Name = "회사명")]
        public string VisitorCompany { get; set; }

        [Required]
        [Display(Name = "접견자 ID")]
        public int HostID { get; set; }

        [Required]
        [Display(Name = "접견자")]
        public string HostName { get; set; }

        [Required]
        [Display(Name = "회사명")]
        public string HostCompany { get; set; }

        [Required]
        [Display(Name = "개인정보 동의 1")]
        public bool AgreeFirst { get; set; }

        [Required]
        [Display(Name = "개인정보 동의 1")]
        public char AgreeFirstFlag { get; set; }

        [Required]
        [Display(Name = "개인정보 동의 2")]
        public bool AgreeSecond { get; set; }

        [Required]
        [Display(Name = "개인정보 동의 2")]
        public char AgreeSecondFlag { get; set; }

        [Required]
        [Display(Name = "싸인 저장 장소")]
        public string SignPath { get; set; }

        [Required]
        [Display(Name = "인증번호")]
        public string AuthNumber { get; set; }

        [Required]
        [Display(Name = "내방일자")]
        public DateTime VisitDateTime { get; set; }

        public string CardNo { get; set; }

        public string CardFlag { get; set; }

        public string BlackList { get; set; }

    }
}
