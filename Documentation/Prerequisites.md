### <span style="color:LightGreen">Keywords:</span>
- or
- and
- admission to
- admission in
- admission into
- including
- ###cp  
- Permission by special approval (waiver)
- Completion of
- Corequisite
- WAM of ##

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

   def : STATEMENT OR STATEMENT ;

   OR : 'or' | 'OR' ;
- **and**
   - statement ___and___ statement
   
   def : STATEMENT AND STATEMENT ;

   AND : 'and' | 'AND' ;
 - **admission**
   - (___A___|___a___)___dmission (in|to|into)___ statement
  
   def : HEADER STATEMENT

   HEADER : 'Admission' | 'admission' PREPOSITION ;

   PREPOSITION : 'in' | 'to' | 'into' ;
 - **cp**
   - ##___cp ( (___in___ QUALIFIER (___or___ QUALIFIER) ___units) at___ #### ___level___ (___or above___) (___including___ STATEMENT) | ___from___ statement ___-___ statement)
   - Special case ___or___ in this situation

   def : CP LEVEL*;

   CP : NUM+ 'cp' ;

   NUM : [0-9] ;

   LEVEL : 'at' LVL 'level' 'or above'?   ;

   LVL : NUM '00' | '000' ;

   INCLUDE : 'including' CP 'of' (STATEMENT | UNIT)
   
   UNIT : (); 

### <span style="color:Orange">Older stuff:</span>
 Does not match courses with spaces in name  
 (A|a)dmission to   [A-Z])\w+(\sor ([A-Z\s]\w+))*
 Splits admission strings                     
 (A|a)dmission to (.(?  (A|a)dmission))*

Special conditions that require regex and sanitising before parenthese parsing.

- GradCertAncHist (OUA)
- MAncHist (OUA)
- GradCertFin (OUA)
- M CrWriting (OUA)
- MCrWrit (OUA)
- MTeach (Birth to five)
- M Teaching (0 to 5) (OUA)
- Permission by special approval (waiver)
- BArts (Single or double degrees)
- MPSP (OUA)
- BTeach(ECE)
- BTeach(ECS)


- (P|C|D|HD) for grade?
- PHYS106(D)
- PHYS1020(D)
- (PHYS201(D) or PHYS2010(D)) and (PHYS202(D) or PHYS2020(D))
- WMAT123 (HD)
- 10cp(P)

- GradDipPASR (OUA)
- BABEd(Prim)
- BABEd(Sec)

- MAppFin(Beijing)
