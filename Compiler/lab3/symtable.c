/*
* symtable.c
* progrmmer – 정수진
* date - 2022/04/14.
*/

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>

#define FILE_NAME "test.dat"
#define STsize 1000 //size of string table
#define HTsize 100 //size of hash table

#define isLetter(x) (((x) >= 'a' && (x) <= 'z') || ((x) >= 'A' && (x) <= 'Z'))
#define isDigit(x) ((x) >= '0' && (x) <= '9')

int nextid = 0, nextfree = 0;
int hashcode;
int found;
int idx1, idx2;
char errorStr[1000] = "";
int length;

typedef struct HTentry* HTpointer;
typedef struct HTentry {
    int index;      //index of identifier in ST
    HTpointer next;  //pointer to next identifier
} HTentry;

enum errorTypes { noerror, illsp, illid, overst, longid };
typedef enum errorTypes errortypes;

char seperators[] = " .,;:?!\t\n";

HTpointer HT[HTsize];
char ST[STsize];

// more global variables
errortypes error;

FILE* fp;   //to be a pointer to FILE
char input;

// Skip Seperators - skip over strings of spaces,tabs,newlines, . , ; : ? !
void SkipSeperators()
{
    while (input != EOF) {
        int flag = 0;

        for (int i = 0; i < sizeof(seperators); i++) {
            if (seperators[i] == input) {
                flag = 1;
                break;
            }
        }

        if (flag == 1) input = fgetc(fp);
        else break;
    }
}

// isSeperator
int isSeperator(char c) {
    for (int i = 0; i < sizeof(seperators); i++) {
        if (seperators[i] == c)
            return 1;
    }

    return 0;
}

// PrintHStable     -       Prints the hash table.write out the hashcode and the list of identifiers
//            associated with each hashcode,but only for non-empty lists.
//            Print out the number of characters used up in ST.
void PrintHStable()
{
    printf("\n\n[[ HASH TABLE ]]\n\n");

    for (int i = 0; i < HTsize; i++) {
        HTentry* node = HT[i]; // HT[i] head node

        if (node == NULL) continue;

        printf("Hash Code %2d : ", i);

        while (node != NULL) {
            char ch = ST[node->index];
            while (ch != '\0') {
                printf("%c", ch);
                ch = ST[++node->index];
            }

            printf("\t");
            node = node->next;
        }
        printf("\n");
    }

    int cnt = 0;
    for (int i = 0; i < STsize; i++)
        if (isLetter(ST[i]) || (ST[i] == '_') || isDigit(ST[i]))
            cnt++;

    printf("\n<%d characters are used in the string table>\n", cnt);
}

// PrintIdentifier
void PrintIdentifier() {
    char str[1000] = "";
    int idx = 0;
    for (int i = nextid; i < nextfree; i++)
        str[idx++] = ST[i];
    printf("%-25s", str);
}

// Printerror    -     Print out error messages
//            overst :  overflow in ST
//            illid    : illegal identifier
//            longid   : identifier
void Printerror(errortypes error)
{
    if (error == overst) {
        printf("***error*** OVERFLOW\n");
        PrintHStable();
        exit(0);
    }
    else if (error == illid) {
        printf("%-25s", "***error***");
        PrintIdentifier();
        printf("%s\n", "illegal identifier");
    }
    else if (error == longid) {
        printf("%-25s", "***error***");
        PrintIdentifier();
        printf("%s\n", "too long identifier");
    }
}

// deleteIdentifier
void deleteIdentifier() {
    for (int i = nextfree - 1; i >= nextid; i--) {
        ST[i] = '\0';
    }
}


// ComputeHS     - Compute the hash code of identifier by summing the ordinal values of its
//              characters and then taking the sum modulo the size of HT.
void ComputeHS(int nid, int nfree)
{

    int code, i;
    code = 0;
    for (i = nid; i < nfree - 1; i++)
        code += (int)ST[i];
    hashcode = code % HTsize;
}

// LookupHS     -For each identifier,Look it up in the hashtable for previous occurrence
//            of the identifier.If find a match, set the found flag as true.
//            Otherwise flase.
//            If find a match, save the starting index of ST in same id.
void LookupHS(int nid, int hscode)
{
    found = 0;
    HTentry* node = HT[hscode];
    while (node != NULL) {
        idx1 = node->index;
        idx2 = nid;
        char ch1 = ST[idx1];
        char ch2 = ST[idx2];
        int same = 0;

        while (ch1 != '\0' && ch2 != '\0') {
            if (ch1 == ch2) {
                same = 1;
                ch1 = ST[++idx1];
                ch2 = ST[++idx2];
            }
            else {
                same = 0;
                break;
            }
        }

        if (same == 1) {
            idx1 = node->index;
            found = 1;
            break;
        }
        node = node->next;
    }
}

// ADDHT    -    Add a new identifier to the hash table.
//            If list head ht[hashcode] is null, simply add a list element with
//            starting index of the identifier in ST.
//            IF list head is not a null , it adds a new identifier to the head of the chain
void ADDHT(int hscode)
{
    HTpointer ptr;

    ptr = (HTpointer)malloc(sizeof(ptr));
    ptr->index = nextid;
    ptr->next = HT[hscode];
    HT[hscode] = ptr;
}

/*  MAIN    -    Read the identifier from the file directly into ST.
Compute its hashcode.
Look up the idetifier in hashtable HT[hashcode]
If matched, delete the identifier from STand print ST - index of the matching identifier.
If not matched, add a new element to the list, pointing to new identifier.
Print the identifier, its index in ST, and whether it was entered or present.
Print out the hashtable, and number of characters used up in ST
*/

int symtable(input)
{
    int i;
    PrintHeading();

    while (input != EOF) {
        error = noerror;
        SkipSeperators();
        if (input == EOF) break;
        if (error == noerror) {
            if (nextfree >= STsize) {
                error = overst;
                Printerror(error);
                break;
            }
            ST[nextfree++] = '\0';

            ComputeHS(nextid, nextfree);
            LookupHS(nextid, hashcode);

            if (!found) {
                // print message
                printf("%-25d", nextid);
                PrintIdentifier();
                printf("%s\n", "(entered)");

                ADDHT(hashcode);
            }
            else {
                // print message
                printf("%-25d", idx1);
                PrintIdentifier();
                deleteIdentifier();
                nextfree = nextid;
                printf("%s\n", "(already existed)");
            }
        }
    }
    PrintHStable();
}
