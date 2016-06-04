using System;

namespace func.brainfuck
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Треугольник Серпинского на BF:");
			Brainfuck.Run(@"
                                >    
                               + +    
                              +   +    
                             [ < + +    
                            +       +    
                           + +     + +    
                          >   -   ]   >    
                         + + + + + + + +    
                        [               >    
                       + +             + +    
                      <   -           ]   >    
                     > + + >         > > + >    
                    >       >       +       <    
                   < <     < <     < <     < <    
                  <   [   -   [   -   >   +   <    
                 ] > [ - < + > > > . < < ] > > >    
                [                               [    
               - >                             + +    
              +   +                           +   +    
             + + [ >                         + + + +    
            <       -                       ]       >    
           . <     < [                     - >     + <    
          ]   +   >   [                   -   >   +   +    
         + + + + + + + +                 < < + > ] > . [    
        -               ]               >               ]    
       ] +             < <             < [             - [    
      -   >           +   <           ]   +           >   [    
     - < + >         > > - [         - > + <         ] + + >    
    [       -       <       -       >       ]       <       <    
   < ]     < <     < <     ] +     + +     + +     + +     + +    
  +   .   +   +   +   .   [   -   ]   <   ]   +   +   +   +   +  
", Console.Read, Console.Write);
		}
	}
}