using System;
using System.Collections.Generic;
using System.Text;

namespace JsDataGrids.DataAccess.Models
{
   public class GridPagination
   {
       public int PageSize { get; set; }
       public int CurrentPage { get; set; }
       public string SortColumn { get; set; }
       public string SortOrder { get; set; }
       public string WhereCondition { get; set; }

   }


   public class Ref<T>
   {
       public T Value { get; set; }

       public Ref(){}
       public Ref(T value) { Value = value; }

       public override string ToString()
       {
           T value = Value;
           return value == null ? "" : value.ToString();
       }

       public static implicit operator T(Ref<T> r) { return r.Value; }

       public static implicit operator Ref<T>(T value) { return new Ref<T>(value); }

    }


}
