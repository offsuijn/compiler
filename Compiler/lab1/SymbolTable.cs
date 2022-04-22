/****************************************************************************************************************

Hashtable Implementation (STsize  = 1000)
=========================================




Date		:	dd, mm , yyyy 

Description :

				The input to the program is a file , consisting of identifiers seperated by
				spaces,tab characters,newlines,and punctuation marks: , . ; : ? 
				An identifier is a string of letters and digits,starting with a letter.Case is significant.
				The program reads in each identifier from the input file directly into ST(string table) 
				and append it to the previous identifer,terminated by null charater.
				Compute its hashcode.The hashcode of an identifier is computed by summing the original values
				of its characters and then taking the sum modulo the size of HT.
				Look up the identifier in ST starting with HT[hashcode].If the listhead is nil,simply add a list
				element,the starting index of the identifer in ST.Otherwise search the list for a previous occurrence 
				of the identifier.If not match add a new element to the list, pointing to the new identifier.
				If match,delete the new identifier from ST and print the ST-index of the matching identifier.
				For each identifier encountered,print the identifier and its index in the stringtable,
				wether it was entered or already existed.
				After the program is finished processing its input,print hash table.
				If the ST overflows,prints the hashtable as above and abort by calling the function "exit()".


Input	:		A file consisting of identifiers seperated by spaces,tab characters,newlines and punctuation marks.
				An identifier is a string of letters and digits,starting with a letter.


Output	:	The identifier,its index in the stringtable and whether entered or present.
			Prints error message for illegal identifier(starting with a digit),illegal seperator and over string.
			Prints the hashtable before terminating.Simply write out hashcode and the list of identifiers 
			associated with each hashcode,but only for non-empty lists.Finally,print out the number of 
			characters used up in ST.

Restriction :	If the ST overflows, print the hashtable as above,
				and abort by calling the function "exit()". "exit()" terminates the execution of a program.

Grobal variations : 
				ST - Array of string table
				HT - Array of list head of hashtable
				letters - Set of letters A..Z, a..z
				digits - Set of digits 0..9
				seperators - null , . ; : ? ! \t \n

*****************************************************************************************************************/



# include <stdio.h>
# include <stdlib.h>
# include <string.h>

#define FILE_NAME "testdata.txt"

#define STsize 1000  //size of string table
#define HTsize 100	//size of hash table

#define FALSE 0
#define TRUE 1

#define isLetter(x) ( ((x) >= 'a' && (x) <='z') || ((x) >= 'A' && (x) <= 'Z') )
#define isDigit(x) ( (x) >= '0' && (x) <= '9' )

typedef struct HTentry *HTpointer;
typedef struct HTentry
{
	int index;  //index of identifier in ST
	HTpointer next;  //pointer to next identifier
}
HTentry;


enum errorTypes { noerror, illsp, illid, overst };
typedef enum errorTypes ERRORtypes;

char seperators[] = " .,;:?!\t\n";

HTpointer HT[HTsize];
char ST[STsize];

int nextid = 0;  //the current identifier
int nextfree = 0;  //the next available index of ST
int hashcode;  //hash code of identifier
int sameid;  //first index of identifier

int found;  //for the previous occurrence of an identifie

ERRORtypes err;

FILE* fp;   //to be a pointer to FILE 
char input;



//Initialize - open input file

void initialize()
{
	fp = fopen(FILE_NAME, "r");
	input = fgetc(fp);
}


//isSerperator  -  distinguish the seperator
int isSeperator(char c)
{
	int i;
	int sep_len;

	sep_len = strlen(seperators);
	for (i = 0; i < sep_len; i++)
	{
		if (c == seperators[i])
			return 1;
	}
	return 0;
}

//printHeading	 -		Print the heading

void PrintHeading()
{
	printf("\n\n");
	printf("  -----------      ------------ \n");
	printf("  Index in ST       identifier  \n");
	printf("  -----------      ------------ \n");
	printf("\n");
}



/*PrintHStable     -   	Prints the hash table.write out the hashcode and the list of identifiers 
						associated with each hashcode,but only for non-empty lists.
						Print out the number of characters used up in ST. */

void PrintHStable()
{

}




/* PrintError    - 	Print out error messages
			overst :  overflow in ST
			print the hashtable and abort by calling the function "abort()".
			illid    : illegal identifier
			illsp    :illegal seperator*/

void PrintError(ERRORtypes err)
{
	switch (err)
	{
		case overst:
			printf("...Error...   OVERFLOW ");
			PrintHStable();
			exit(0);
			break;
		case illsp:
			printf("...Error...  %c is illegal seperator \n", input);
			break;
		case illid:
			printf("...Error... ");
			while (input != EOF && (isLetter(input) || isDigit(input)))
			{
				printf("%c", input);
				input = fgetc(fp);
			}
			printf(" start with digit \n");
			break;
	}
}



/* Skip Seperators -   	skip over strings of spaces,tabs,newlines, . , ; : ? !
						if illegal seperators,print out error message.*/

void SkipSeperators()
{
	while (input != EOF && !(isLetter(input) || isDigit(input)))
	{
		if (!isSeperator(input))
		{
			err = illsp;
			PrintError(err);
		}
		input = fgetc(fp);
	}
}



/*ReadID 	- 	Read identifier from the input file the string table ST directly into
			ST(append it to the previous identifier).
			An identifier is a string of letters and digits, starting with a letter.
			If first letter is digit, print out error message. */

void ReadID()
{

}



/* ComputeHS 	- 	Compute the hash code of identifier by summing the ordinal values of its
					characters and then taking the sum modulo the size of HT. */

void ComputeHS(int nid, int nfree)
{
	int code, i;
	code = 0;
	for (i = nid; i < nfree - 1; i++)
		code += (int)ST[i];
	hashcode = code % HTsize;
}



/*LookupHS 	-	For each identifier,Look it up in the hashtable for previous occurrence
				of the identifier.If find a match, set the found flag as true.
				Otherwise flase.
				If find a match, save the starting index of ST in same id. */

void LookupHS(int nid, int hscode)
{

}


/* ADDHT	-	Add a new identifier to the hash table.
			If list head ht[hashcode] is null, simply add a list element with
			starting index of the identifier in ST.
			IF list head is not a null , it adds a new identifier to the head of the chain */

void ADDHT(int hscode)
{
	HTpointer ptr;

	ptr = (HTpointer)malloc(sizeof(ptr));
	ptr->index = nextid;
	ptr->next = HT[hscode];
	HT[hscode] = ptr;
}


/* MAIN		-	Read the identifier from the file directly into ST.
			Compute its hashcode.
			Look up the idetifier in hashtable HT[hashcode]
 		 	If matched, delete the identifier from ST and print ST-index
			of the matching identifier.
			If not matched , add a new element to the list,pointing to new identifier.
			Print the identifier,its index in ST, and whether it was entered or present.
			Print out the hashtable,and number of characters used up in ST
*/
int main()
{
	int i;
	PrintHeading();
	initialize();

	while (input != EOF)
	{
		err = noerror;
		SkipSeperators();
		ReadID();
		if (input != EOF && err != illid)
		{
			if (nextfree == STsize)
			{
				err = overst;
				PrintError(err);
			}
			ST[nextfree++] = '\0';

			ComputeHS(nextid, nextfree);
			LookupHS(nextid, hashcode);

			if (!found)
			{
				printf("%6d         ", nextid);
				for (i = nextid; i < nextfree - 1; i++)
					printf("%c", ST[i]);
				printf("          (entered)\n");
				ADDHT(hashcode);
			}
			else
			{
				printf("%6d         ", sameid);
				for (i = nextid; i < nextfree - 1; i++)
					printf("%c", ST[i]);
				printf("          (already existed)\n");
				nextfree = nextid;
			}
		}
	}
	PrintHStable();
}
