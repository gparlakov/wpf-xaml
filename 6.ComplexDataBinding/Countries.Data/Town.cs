//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Countries.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Town
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
    
        public virtual Country Country { get; set; }
    }
}
