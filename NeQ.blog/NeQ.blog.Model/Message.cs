using NeQ.blog.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeQ.blog.Model
{
    public class Message:IEntity
    {
        [Key]public Guid ID { get; set; }
        public Guid SelfId { get; set; }
        public Guid MsgId { get; set; }
        public Guid OmsgId { get; set; }

    }
}
