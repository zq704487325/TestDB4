using System;
using Ruanmou.Common.Attributees;
namespace Ruanmou.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Company : BaseModel
    {

        [ShowChinese("��˾��")]
        public string Name { get; set; }
        [ShowChinese("����ʱ��")]
        public System.DateTime CreateTime { get; set; }
        [ShowChinese("�����˵�ID")]
        public int CreatorId { get; set; }
        [ShowChinese("���һ���޸��˵�ID")]
        public int? LastModifierId { get; set; }
        [ShowChinese("���һ���޸ĵ�ʱ��")]
        public DateTime? LastModifyTime { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [DBName("Company")]
    public class CompanyModel : BaseModel
    {
        [DBName("Name")]
        [ShowChinese("��˾��")]
        public string Name_ { get; set; }
        [DBName("CreateTime")]
        [ShowChinese("����ʱ��")]
        public System.DateTime CreateTime_ { get; set; }
        [DBName("CreatorId")]
        [ShowChinese("�����˵�ID")]
        public int CreatorId_ { get; set; }
        [DBName("LastModifierId")]
        [ShowChinese("���һ���޸��˵�ID")]
        public int? LastModifierId_ { get; set; }
        [DBName("LastModifyTime")]
        [ShowChinese("���һ���޸ĵ�ʱ��")]
        public DateTime? LastModifyTime_ { get; set; }
    }


}