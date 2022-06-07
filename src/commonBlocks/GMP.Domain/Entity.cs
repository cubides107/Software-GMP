using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Domain
{
    public abstract class Entity
    {
        public string Id { get; protected set; }

        protected Entity(Guid? id = null)
        {
            this.Id = id != null ? id.ToString() : Guid.NewGuid().ToString();
        }
    }
}
