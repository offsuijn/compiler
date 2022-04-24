%{
    /*
    * scanner.l – lexical analyzer for the MiniC
    * progrmmer – 정수진
    * date - 2022/04/14.
    */
    #include <stdio.h>
    #include <stdlib.h>
    #include "tn.h"
    #include "glob.h"
%}
%%

"const"					return(TCONST);
"else"					return(TELSE);
"if"					return(TIF);
"int"					return(TINT);
"return"				return(TRETURN);
"void"					return(TVOID);
"while"					return(TWHILE);
"=="					return(TEQUAL);
"!="					return(TNOTEQU);
"<="					return(TLESS);
">="					return(TGREAT);
"&&"					return(TAND);
"||"					return(TOR);
"++"					return(TINC);
"--"					return(TDEC);
"+="					return(TADDASSIGN);
"-="					return(TSUBASSIGN);
"*="					return(TMULASSIGN);
"/="					return(TDIVASSIGN);
"%="					return(TMODASSIGN);
[A-Za-z_][A-Za-z0-9_]*			return(TIDENT);
[1-9][0-9]*0([0-7]+|(x|X)[0-9A-Fa-f]*)?	return(TNUMBER);
"/*"([^*]|\*+[^*/])*\**"*/"		;
"//".*					;
[ \t\n]					;
.					return(TERROR);
%%

int yywrap()
{
	printf(" 1976355\n");
	return 1;
}
