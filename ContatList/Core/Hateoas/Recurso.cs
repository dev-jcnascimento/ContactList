using System.Collections.Generic;
using ContactList.Core.Hateoas;

namespace ContactList.Core.Hateoas
{
    public class Recurso
    {
        public List<LinkDTO> Links { get; set; } = new List<LinkDTO>();

    }
}
