using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// 김남훈 최초생성 
/// 데이터 베이스에서 핸들링할 모델
/// </summary>
namespace VisitorSystem.Models
{
        /// <summary>
        /// Table Visitor 모델
        /// </summary>
        public class Visitor
        {
            //내방객 ID
            public int VisitorID { get; set; }

            //내방객 이름
            public string VisitorName { get; set; }

            //내방객 폰번호
            [DataType(DataType.PhoneNumber)]
            public string Mobile { get; set; }

            //내방객 회사
            public string Company { get; set; }

            //마지막 내방일자
            [DataType(DataType.DateTime)]
            public DateTime LastVisitDate { get; set; }

            public string BlackList { get; set; }

            //삭제여부
            public char DeleteFlag { get; set; }

        }

        /// <summary>
        /// Table VisitorBlackList 모델
        /// </summary>
        public class VisitorBlackList
        {
            //블랙리스트 내방객 ID
            [Required]
            public int VisitorID { get; set; }

            //블랙리스트 내방객 이름
            public string VisitorName { get; set; }

            //블랙리스트 내방객 회사
            public string Company { get; set; }

            //블랙리스트 내방객 연락처
            public string Mobile { get; set; }

            //블랙리스트 등록일
            [DataType(DataType.DateTime)]
            public DateTime InsertDate { get; set; }

            //블랙리스트 등록한 운영자 ID
            public string AdminID { get; set; }
            
            //블랙리스트 등록사유
            public string Desc { get; set; }
        }

        /// <summary>
        /// Table VisitorHistory 모델
        /// </summary>
        public class VisitorHistory
        {
            //내방객 ID
            [Required]
            public int VisitorID { get; set; }

            //개인정보 동의 첫번째
            public char AgreeFlagFirst { get; set; }

            //개인정보 동의 두번째
            public char AgreeFlagSecond { get; set; }

            //개인정보 동의 사인 파일 위치
            public string SignPath { get; set; }

            //내방일자
            [Required]
            public DateTime VisitDate { get; set; }

            //발급된 카드 번호
            public string CardNo { get; set; }

            //카드 상태 (발급 I,회수 R, 미회수 N)
            public char CardFlag { get; set; }

            //내방객 특이사항 메모
            public string VisitorDesc { get; set; }

        }

        /// <summary>
        /// Table Host 모델
        /// </summary>
        public class Host
        {
            //접견인 ID
            [Required]
            public int HostID { get; set; }

            //접견인 이름
            public string HostName { get; set; }

            //접견인 회사
            public string Company { get; set; }

            //접견인 핸드폰 번호
            public string Mobile { get; set; }

            //접견인 내선번호
            public string Tel { get; set; }

            //접견인 직급
            public string GradeName { get; set; }

            //접견인 등록 일자
            [DataType(DataType.DateTime)]
            public DateTime InsertDate { get; set; }
        }

        /// <summary>
        /// Table AdminUser 모델
        /// </summary>
        public class AdminUser
        {
            //운영자 ID
            [Required(ErrorMessage = "아이디를 입력해주세요")]
            [Display(Name = "ID")]
            public string AdminID { get; set; }

            //운영자 비밀번호
            [Required(ErrorMessage = "비밀번호를 입력해주세요")]
            [Display(Name = "PW")]
            [DataType(DataType.Password)]
            public string AdminPW { get; set; }

            [Required(ErrorMessage = "비밀번호를 입력해주세요")]
            [Display(Name = "비밀번호")]
            [DataType(DataType.Password)]
            public string AdminNewPW { get; set; }

            [Required(ErrorMessage = "비밀번호를 입력해주세요")]
            [Compare("AdminNewPW" , ErrorMessage = "비밀번호가 다릅니다")]
            [Display(Name = "비밀번호확인")]
            [DataType(DataType.Password)]
            public string AdminNewRePW { get; set; }

            //운영자 마지막 로그인 일자
            public DateTime LastLoginDate { get; set; }

            //운영자 마지막 비밀번호 변경 일자
            public DateTime LastChangePWDate { get; set; }

            //운영자 사용여부
            public string DeleteFlag { get; set; }

            //운영자 등록일자
            public string InsertDate { get; set; }
        }

        /// <summary>
        /// Table AdminLog 모델
        /// </summary>
        public class AdminLog
        {
            //운영자 ID
            [Required]
            public string AdminID { get; set; }

            //운영자 Action 시간
            [Required]
            public DateTime DateTime { get; set; }

            //운영자 Action
            [Required]
            public string Action { get; set; }

            //운영자 로그인 위치
            public int LocationID { get; set; }
        }

        /// <summary>
        /// Table Location 모델
        /// </summary>
        public class Location
        {
            //위치 ID
            [Required]
            public int LocationID { get; set; }

            //위치 이름
            public string LocationName { get; set; }

            //위치 IP
            public string LocationIP { get; set; }

            //위치 등록 일자
            [DataType(DataType.DateTime)]
            public DateTime InsertDate { get; set; }

            //위치별 동작 플래그 (T : 태블릿, A : 관리자)
            public char LocationFlag { get; set; }
        }   
    
        /// <summary>
        /// Table GlobalConfig 모델
        /// </summary>
        public class GlobalConfig
        {
            //설정 ID
            public int ConfigID { get; set; }

            //설정 이름
            [Required]
            public string ConfigName { get; set; }

            //설정 값
            public string ConfigValue { get; set; }
        }


        /// <summary>
        /// Table Card 모델
        /// </summary>
        public class Card
        {
            //설정 ID
            public string CardID { get; set; }

            //설정 이름
            public decimal CardNo { get; set; }

            //설정 값
            public DateTime InsertDate { get; set; }

            //설정 값
            public char CardFlag { get; set; }


            //카드 누가 발급받았나 알려고 미리 만들었는데... 테이블에 녹이는게 좋을꺼같은데;; 시간이 없군
            public int VisitorHistorySeq { get; set; }

            public string VisitorName { get; set; }
    }

}