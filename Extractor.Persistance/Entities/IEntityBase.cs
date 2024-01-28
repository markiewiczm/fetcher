using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Repository.Entities
{
    public interface IEntityBase<TKey>
    {
        TKey Id { get; set; }
    }
}
