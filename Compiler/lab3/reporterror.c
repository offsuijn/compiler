/*
* reporterror.c
* progrmmer – 정수진
* date - 2022/04/14.
*/
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
