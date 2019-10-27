using WebAppCore.Data.Interfaces;
using WebAppCore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAppCore.Data.Enums;

namespace WebAppCore.Data.Entities
{
    [Table("SystemConfigs")]
    public class SystemConfig : DomainEntity<int>
    {
        public string Title { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public string Copyright { get; set; }
        public string Author { get; set; }
        public string Logo { get; set; }
    }
}
