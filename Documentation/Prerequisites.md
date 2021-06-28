### <span style="color:LightGreen">Keywords:</span>
- or
- and
- admission
- including
- ###cp | ###cps | ## credit points [Normalised to ### cp]
- Permission by special approval (waiver)

### <span style="color:LightGreen">Grammars:</span>
- ( ) Parenthese denote nested statement
- **unit code**
  - 2020+
    - {4}[A-Z]####
      - COMP1000
  - 2019
    - {4}[A-Z]###
      - COMP123
- **or**
   - statement ___or___ statement
- **and**
   - statement ___and___ statement
 - **admission**
   - (___A___|___a___)___dmission to___ statement
 - **cp**
   - ##___cp ( (___in___ QUALIFIER (___or___ QUALIFIER) ___units) at___ #### ___level___ (___or above___) (___including___ STATEMENT) | ___from___ statement ___-___ statement)
   - Special case ___or___ in this situation


/*
   EDTE3010 - has larger pre-requsite chain
   (Admission to BEd(Prim) and (EDUC258 or EDUC2580) and (EDUC260 or EDUC2600) and (EDUC267 or EDUC2670)) or (130cp including (EDUC258 or EDUC2580) and (EDUC260 or EDUC2600) and (EDUC267 or EDUC2670) and (EDTE353 or EDTE3530))

   Does not match courses with spaces in name   (A|a)dmission to ([A-Z])\w+(\sor ([A-Z\s]\w+))*
   Splits admission strings                     (A|a)dmission to (.(?!(A|a)dmission))*



   PICX8060 - gives some stack overflow errors
*/


Keywords:

Rules:

Admission to Course (or Course)


//Special conditions that require regex and sanitising before parenthese parsing.

GradCertAncHist (OUA)
MAncHist (OUA)
GradCertFin (OUA)
M CrWriting (OUA)
MCrWrit (OUA)
MTeach (Birth to five)
M Teaching (0 to 5) (OUA)
Permission by special approval (waiver)
BArts (Single or double degrees)
MPSP (OUA)
BTeach(ECE)
BTeach(ECS)


//(P|C|D|HD) for grade?
PHYS106(D)
PHYS1020(D)
(PHYS201(D) or PHYS2010(D)) and (PHYS202(D) or PHYS2020(D))
WMAT123 (HD)
10cp(P)

GradDipPASR (OUA)
BABEd(Prim)
BABEd(Sec)

MAppFin(Beijing)
