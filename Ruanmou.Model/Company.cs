using System;
using Ruanmou.Common.Attributees;
namespace Ruanmou.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Company : BaseModel
    {

        [ShowChinese("公司名")]
        public string Name { get; set; }
        [ShowChinese("创建时间")]
        public System.DateTime CreateTime { get; set; }
        [ShowChinese("创建人的ID")]
        public int CreatorId { get; set; }
        [ShowChinese("最后一次修改人的ID")]
        public int? LastModifierId { get; set; }
        [ShowChinese("最后一次修改的时间")]
        public DateTime? LastModifyTime { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [DBName("Company")]
    public class CompanyModel : BaseModel
    {
        [DBName("Name")]
        [ShowChinese("公司名")]
        public string Name_ { get; set; }
        [DBName("CreateTime")]
        [ShowChinese("创建时间")]
        public System.DateTime CreateTime_ { get; set; }
        [DBName("CreatorId")]
        [ShowChinese("创建人的ID")]
        public int CreatorId_ { get; set; }
        [DBName("LastModifierId")]
        [ShowChinese("最后一次修改人的ID")]
        public int? LastModifierId_ { get; set; }
        [DBName("LastModifyTime")]
        [ShowChinese("最后一次修改的时间")]
        public DateTime? LastModifyTime_ { get; set; }
    }


}