using System.ComponentModel.DataAnnotations.Schema;
using eCommerceAutomation.API.Framework.Constants;

namespace eCommerceAutomation.API.Domain
{
    [Table("source_name", Schema = "product")]
    public class SourceName : Entity
    {
        [Column("source_type")]
        public SourceType SourceType
        {
            get;
            set;
        }

        [Column("key")]
        public string Key
        {
            get;
            set;
        }
    }
}
