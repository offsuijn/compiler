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
