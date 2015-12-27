using System.Text;

public static class StringExtensions 
{ 
   /// <summary> 
   /// Humanize a "CamelCasedString" into "Camel Cased String". 
   /// </summary> 
   /// <param name="source"></param> 
   /// <returns></returns> 
   public static string Humanize(this string source ) 
    { 
       StringBuilder sb =new StringBuilder(); 

       char last = char.MinValue; 
       foreach (char c in source ) 
        { 
           if (char.IsLower( last ) ==true &&char.IsUpper( c ) ==true ) 
            { 
                sb.Append(' '); 
            } 
            sb.Append( c ); 
            last = c; 
        } 
       return sb.ToString(); 
    } 
}
