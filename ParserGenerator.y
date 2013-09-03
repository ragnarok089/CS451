%{
    Dictionary nt = new Dictionary<string,int>();
%}

%start file

/*
 * The accessibility of the Parser object must not exceed that
 * of the inherited ShiftReduceParser<,>. Thus if you want to include 
 * the *source* of ShiftReduceParser from ShiftReduceParserCode.cs, 
 * then you must either set the compilation flag EXPORT_GPPG or  
 * override the default, public visibility with %visibility internal.
 * If you reference the pre-compiled QUT.ShiftReduceParser.dll then 
 * ShiftReduceParser<> is public and either visibility will work.
 */
%visibility internal

%union { public string sVal; 
		 public Stmt stmtVal; 
		 public Piece pVal;
		 public Pieces psVal;
		 public PieceDef pdVal;
		 public ImportedFile ifVal;
		 public Win wVal;
		 public Eot eVal;
		 public Move mVal;
         public int iVal; }

%token <iVal> NUMBER 
%token <sVal> STRING
%token <sVal> GREATER
%token <sVal> GREATEREQ
%token <sVal> EQUAL
%token <sVal> LESS
%token <sVal> LESSEQ
%token <sVal> AND
%token <sVal> OR
%token <sVal> WHILE
%token <sVal> PIECE
%token <sVal> START
%token <sVal> END
%token <sVal> THIS
%token <sVal> IF
%token <sVal> ELSE
%token <sVal> BOARD
%token <sVal> EOT
%token <sVal> WIN
%token <sVal> STRING
%token <sVal> DIRECTION
%token <sVal> X
%token <sVal> Y
%token <sVal> REPLACE
%token <sVal> TYPE
%token <sVal> HASMOVED
%token <sVal> EOT
%token <sVal> WIN
%token <sVal> PIECEDEF

%type <Expr> element
%type <Expr> expr
%type <Expr> term
%type <Expr> fact
%type <Expr> strcmp
%type <stmtVal> while
%type <stmtVal> if
%type <stmtVal> assign
%type <pVal> piece
%type <stmtVal> stmt
%type <stmtlistVal> stmtlist
%type <ifVal> file
%type <psVal> piece
%type <pdVal> piecedef
%type <mVal> move
%type <eVal> eot
%type <wVal> win

%%

file 	:	pieces ':' startingposlist ':' eot ':' win ':'
				{
					$$=new ImportedFile($1,$3,$5,$7);
				}
		;
startingposlist	:	startingpos
				{
					StartingPosList spl=new StartingPosList();
					spl.add($1);
					$$=spl;
				}
			|	startingposlist startingpos
				{
					$1.add($2);
					$$=$1;
				}
		;
startingpos	:	NUMBER ':' NUMBER ':' STRING ':' NUMBER ':'
				{
					$$=new StartingPos($1,$3,$5,$7);
				}
		;
pieces 	:	piecedef
				{
					Pieces p = new Pieces();
					p.add($1);
					$$=p;
				}
		|	pieces ':' piecedef ':'
				{
					$1.add($3);
					$$=$1;
				}
		;
piecedef	:	PIECEDEF ':' STRING ':' move ':'
				{
					$$=new PieceDef($3,$5);
				}
		;
move	:	stmtlist
				{
					$$=new Move($1);
				}
		;
eot	:	EOT ':' stmtlist ':'
				{
					$$=new Eot($3);
				}
		;
win	:	WIN ':' stmtlist ':'
				{
					$$=new Win($3);
				}
		;
piece	:	BOARD '[' expr ']' '[' expr ']'
				{
					$$=new BoardElement($3,$6);
				}
		|	THIS
				{
					$$=new This();
				}
		;

while	:	WHILE '(' expr ')' START stmtlist END
				{
					$$=new WhileStmt($3,$6);
				}
		;
assign	:	STRING '=' expr
				{
					$$=new AssignStmt($1,$3);
				}
		;
if	:	IF '(' expr ')' START stmtlist END
				{
					$$=new IfStmt($3,$6,new StmtList());
				}
			|
			IF '(' expr ')' START stmtlist ELSE stmtlist END
				{
					$$=new IfStmt($3,$6,$8);
				}
		;		
stmtlist	:	stmt stmtlist
				{
					$2.insert($1);
					$$=$2;
				}
			|	stmt
				{
					StmtList sl=new StmtList();
					sl.insert($1);
					$$=sl;
				}
		;
		
stmt	:	while
				{
					$$=$1;
				}
			| if
				{
					$$=$1;
				}
			| assign ';'
				{
					$$=$1;
				}
			| piece '.' REPLACE '(' STRING ')' ';'
				{
					$$=new ReplaceStmt($1,$5);
				}
		;
expr	: expr OR element
			{
				$$=new Or($1,$3);
			}
		| expr AND element
			{
				$$=new And($1,$3);
			}
		| expr GREATER element
			{
				$$=new Greater($1,$3);
			}
		| expr LESS element
			{
				$$=new Less($1,$3);
			}
		| expr EQUAL element
			{
				$$=new Equ($1,$3);
			}
		| expr GREATEREQ element
			{
				$$=new GreaterEq($1,$3);
			}
		| expr LESSEQ element
			{
				$$=new LessEq($1,$3);
			}
		| element
			{
				$$ = $1;
			}
		| strcmp
			{
				$$ = $1;
			}
		;
element	:	element '+' term
			{
				$$=new Plus($1,$3);
			}
		|
			element '-' term
			{
				$$=new Minus($1,$3);
			}
		| 	term
			{
				$$=$1;
			}
		;
		
term	:	term '*' fact
			{
				$$=new Times($1,$3);
			}
		|
			term '/' fact
			{
				$$=new Divide($1,$3);
			}
		|
			term '%' fact
			{
				$$=new Mod($1,$3);
			}
		|
			fact
			{
				$$=$1;
			}
		;
fact	:	'(' expr ')'
			{
				$$=$2;
			}
		|
			STRING
			{
				$$=new Ident($1);
			}
		|
			NUMBER
			{
				$$=new Number($1);
			}
		|	piece '.' X
			{
				$$=new XVal($1,$3);
			}
		|	piece '.' Y
			{
				$$=new YVal($1,$3);
			}
		|	piece '.' DIRECTION
			{
				$$=new Direction($1,$3);
			}
		|	piece '.' HASMOVED
			{
				$$=new HASMOVED($1);
			}
		;
strcmp	: piece '.' TYPE EQUAL piece '.' TYPE
			{
				$$=new StrCmp(new Type($1),new Type($5));
			}
		| piece '.' TYPE EQUAL '\"' STRING '\"'
			{
				$$=new StrCmp(new Type($1),$6);
			}
		|  '\"' STRING '\"' EQUAL piece '.' TYPE
			{
				$$=new StrCmp($2,new Type($5));
			}
		| '\"' STRING '\"' EQUAL '\"' STRING '\"'
			{
				$$=new StrCmp($2,$6);
			}
		;
%%


class ImportedFile
{
	public Pieces _p;
	public StartingPos _sp;
	public Eot _eot;
	public Win _win;
	public ImportedFile(Pieces p, StartingPos sp, Eot eot, Win win)
	{
		_p=p;
		_sp=sp;
		_eot=eot;
		_win=win;
	}
	
}
class StartingPosList
{
	List<StartingPos> _pos;
	
	public StartingPosList()
	{
		_pos=new List<StartingPos>();
	}
	
	public void add(StartingPos s)
	{
		_pos.Add(s);
	}
	
	public getPieces()
	{
		return _pos;
	}
	
}
class StartingPos
{
	public int _x;
	public int _y;
	public string _type;
	public int _direction;
	public starting(int x, int y, string type, int direction)
	{
		_x=x;
		_y=y;
		_type=type;
		_direction=direction;
	}
}
class Pieces
{
	List<subPiece> _pieces;
	
	public Pieces()
	{
		_pieces=new List<subPiece>();
	}
	
	public void add(subPiece s)
	{
		_pieces.Add(s);
	}
	
	public getPieces()
	{
		return _pieces;
	}
	
}

class subPiece 
{
	string _type;
	Move _move;
	public subPieces(string type, Move move)
	{
		_type=type;
		_move=move;
	}
	public getType()
	{
		return _type;
	}
	public getMove()
	{
		return _move;
	}
}

class Eot
{
	StmtList _sl;
	
	public Eot(StmtList sl)
	{
		_sl=sl;
	}
	public bool eval(Board board,Dictionary nt)
	{
		sl.eval(board,nt);
		if(nt.Containskey("return") && nt["return"]==1){
			return true;
		}
		return false;
	}
}

class Move
{
	StmtList _sl;
	
	public Move(StmtList sl)
	{
		_sl=sl;
	}
	public bool eval(Board board,Dictionary nt,Piece t,int x, int y)
	{
		nt["movex"]=x;
		nt["movey"]=y;
		sl.eval(board,nt,t);
		if(nt.Containskey("return") && nt["return"]==1){
			return true;
		}
		return false;
	}
}

class Win
{
	StmtList _sl;
	
	public Win(StmtList sl)
	{
		_sl=sl;
	}
	public int eval(Board board,Dictionary nt)
	{
		sl.eval(board,nt);
		if(nt.Containskey("winner")){
			return nt["winner"];
		}
		return 0;
	}
}

abstract class Expr
{
	abstract public int eval(Board board,Dictionary nt,Piece t);
}

class Equ : Expr
{
	Expr _lhs;
	Expr _rhs;
	public Equ(Expr lhs, Expr rhs)
	{
		_lhs=lhs;
		_rhs=rhs;
	}
	
	public int eval(Board board,Dictionary nt,Piece t)
	{
		if(_lhs.eval(nt,board,t) == _rhs.eval(nt,board,t))
		{
			return 1;
		}
		return 0;
	}
}

class Greater : Expr
{
	Expr _lhs;
	Expr _rhs;
	public Greater(Expr lhs, Expr rhs)
	{
		_lhs=lhs;
		_rhs=rhs;
	}
	
	public int eval(Board board,Dictionary nt,Piece t)
	{
		if(_lhs.eval(nt,board,t) > _rhs.eval(nt,board,t))
		{
			return 1;
		}
		return 0;
	}
}

class GreaterEq : Expr
{
	Expr _lhs;
	Expr _rhs;
	public GreaterEq(Expr lhs, Expr rhs)
	{
		_lhs=lhs;
		_rhs=rhs;
	}
	
	public int eval(Board board,Dictionary nt,Piece t)
	{
		if(_lhs.eval(nt,board,t) >= _rhs.eval(nt,board,t))
		{
			return 1;
		}
		return 0;
	}
}
class Less : Expr
{
	Expr _lhs;
	Expr _rhs;
	public Less(Expr lhs, Expr rhs)
	{
		_lhs=lhs;
		_rhs=rhs;
	}
	
	public int eval(Board board,Dictionary nt,Piece t)
	{
		if(_lhs.eval(nt,board,t) < _rhs.eval(nt,board,t))
		{
			return 1;
		}
		return 0;
	}
}

class LessEq : Expr
{
	Expr _lhs;
	Expr _rhs;
	public LessEq(Expr lhs, Expr rhs)
	{
		_lhs=lhs;
		_rhs=rhs;
	}
	
	public int eval(Board board,Dictionary nt,Piece t)
	{
		if(_lhs.eval(nt,board,t) <= _rhs.eval(nt,board,t))
		{
			return 1;
		}
		return 0;
	}
}

class Or : Expr
{
	Expr _lhs;
	Expr _rhs;
	public Or(Expr lhs, Expr rhs)
	{
		_lhs=lhs;
		_rhs=rhs;
	}
	
	public int eval(Board board,Dictionary nt,Piece t)
	{
		if(_lhs.eval(nt,board,t) >0 || _rhs.eval(nt,board,t)>0)
		{
			return 1;
		}
		return 0;
	}
}

class And : Expr
{
	Expr _lhs;
	Expr _rhs;
	public And(Expr lhs, Expr rhs)
	{
		_lhs=lhs;
		_rhs=rhs;
	}
	
	public int eval(Board board,Dictionary nt,Piece t)
	{
		if(_lhs.eval(nt,board,t) >0 && _rhs.eval(nt,board,t)>0)
		{
			return 1;
		}
		return 0;
	}
}

class Number : Expr
{
	int _value;
	public Number(int value)
	{
		_value=value;
	}
	
	public int eval(Board board,Dictionary nt,Piece t)
	{
		return _value;
	}
}

class Ident : Expr
{
	int _name;
	public Ident(string name)
	{
		_name=name;
	}
	
	public int eval(Board board,Dictionary nt,Piece t)
	{
		return nt[_name];
	}
}

class Times : Expr
{
	Expr _lhs;
	Expr _rhs;
	public Times(Expr lhs, Expr rhs)
	{
		_lhs=lhs;
		_rhs=rhs;
	}
	
	public int eval(Board board,Dictionary nt,Piece t)
	{
		return _lhs.eval(nt,board,t) * _rhs.eval(nt,board,t);
	}
}

class Plus : Expr
{
	Expr _lhs;
	Expr _rhs;
	public Plus(Expr lhs, Expr rhs)
	{
		_lhs=lhs;
		_rhs=rhs;
	}
	
	public int eval(Board board,Dictionary nt,Piece t)
	{
		return _lhs.eval(nt,board,t) + _rhs.eval(nt,board,t);
	}
}

class Minus : Expr
{
	Expr _lhs;
	Expr _rhs;
	public Minus(Expr lhs, Expr rhs)
	{
		_lhs=lhs;
		_rhs=rhs;
	}
	
	public int eval(Board board,Dictionary nt,Piece t)
	{
		return _lhs.eval(nt,board,t) - _rhs.eval(nt,board,t);
	}
}

class Divide : Expr
{
	Expr _lhs;
	Expr _rhs;
	public Divide(Expr lhs, Expr rhs)
	{
		_lhs=lhs;
		_rhs=rhs;
	}
	
	public int eval(Board board,Dictionary nt,Piece t)
	{
		return (int)(_lhs.eval(nt,board,t) / _rhs.eval(nt,board,t));
	}
}

class Mod : Expr
{
	Expr _lhs;
	Expr _rhs;
	public Mod(Expr lhs, Expr rhs)
	{
		_lhs=lhs;
		_rhs=rhs;
	}
	
	public int eval(Board board,Dictionary nt,Piece t)
	{
		return (int)(_lhs.eval(nt,board,t) % _rhs.eval(nt,board,t));
	}
}

abstract class Stmt
{
	abstract public void eval(Board board,Dictionary nt,Piece t);
}

class AssignStmt : Stmt
{
	string _name;
	Expr _rhs;
	public AssignStmt(string name, Expr rhs)
	{
		_name=name;
		_rhs=rhs;
	}
	
	public void eval(Board board,Dictionary nt,Piece t)
	{
		nt[_name]=_rhs.eval(nt,board,t);
	}
}

class IfStmt : Stmt
{
	Expr _cond;
	StmtList _tBody;
	StmtList _fBody;
	public IfStmt(Expr cond, StmtList tBody, StmtList fBody)
	{
		_cond=cond;
		_tBody=tBody;
		_fBody=fBody;
	}
	
	public void eval(Board board,Dictionary nt,Piece t)
	{
		if(_cond.eval(nt,board,t)>0)
		{
			_tBody.eval(nt,board,t);
		}
		else
		{
			_fBody.eval(nt,board,t);
		}
	}
}

class ReplaceStmt : Stmt
{
	Piece _p;
	string _str;
	public ReplaceStmt(Piece p,string str)
	{
		_p=p;
		_str=str;
	}
	
	public void eval(Board board,Dictionary nt,Piece t)
	{
		p.Replace(_str);
	}
}

class WhileStmt : Stmt
{
	Expr _cond;
	StmtList _body;
	
	public IfStmt(Expr cond, StmtList body)
	{
		_cond=cond;
		_body=body;
	}
	
	public void eval(Board board,Dictionary nt,Piece t)
	{
		while(_cond.eval(nt)>0)
		{
			body.eval(nt,board,t);
		}
	}
}

class StmtList
{
	List<Stmt> _stmts;
	
	public StmtList()
	{
		_stmts=new List<Stmt>();
	}
	
	public void insert(Stmt s)
	{
		_stmts.Add(s);
	}
	
	public void eval(Board board,Dictionary nt,Piece t)
	{
		foreach (Stmt s in _stmts)
		{
			s.eval(nt,board,t);
		}
	}
}

class This
{	
	public This()
	{
	}
	
	public Piece eval(Board board,Dictionary nt,Piece t)
	{
		return t;
	}
}

class BoardElement
{	
	Expr _x;
	Expr _y;
	
	public BoardElement(Expr x,Expr y)
	{
		_x=x;
		_y=y;
	}
	
	public Piece eval(Board board,Dictionary nt,Piece t)
	{
		return board[_x.eval(board,nt,t)][_y.eval(board,nt,t)].piece;
	}
}

class XVal : Expr
{
	Piece _p;
	public XVal(Piece p)
	{
		_p=p;
	}
	
	public int eval(Board board,Dictionary nt,Piece t)
	{
		if(_p==null)
			return -1;
		return _p.GetX();
	}
}
class YVal : Expr
{
	Piece _p;
	public YVal(Piece p)
	{
		_p=p;
	}
	
	public int eval(Board board,Dictionary nt,Piece t)
	{
		if(_p==null)
			return -1;
		return _p.GetY();
	}
}

class Direction : Expr
{
	Piece _p;
	public Direction(Piece p)
	{
		_p=p;
	}
	
	public int eval(Board board,Dictionary nt,Piece t)
	{
		if(_p==null)
			return -1;
		return _p.direction;
	}
}

class Type
{
	Piece _p;
	public Type(Piece p)
	{
		_p=p;
	}
	
	public string eval(Board board,Dictionary nt,Piece t)
	{
		if(_p==null)
			return "";
		return _p.type;
	}
}

class HasMoved
{
	Piece _p;
	public HasMoved(Piece p)
	{
		_p=p;
	}
	
	public int eval(Board board,Dictionary nt,Piece t)
	{
		if(_p==null)
			return -1;
		if(_p.hasMoved)
			return 1;
		return 0;
	}
}

class StrCmp:Expr
{
	Piece _p=null;
	Piece _p2=null;
	string s=null;
	string s2=null;
	public StrCmp(Piece p,Piece p2)
	{
		_p=p;
		_p2=p2;
	}
	public StrCmp(Piece p,string s)
	{
		_p=p;
		_s=s;
	}
	public StrCmp(string s,Piece p)
	{
		_p=p;
		_s=s;
	}
	public StrCmp(string s,string s2)
	{
		_s=s;
		_s2=s2;
	}
	
	public int eval(Board board,Dictionary nt,Piece t)
	{
		if(p!=null && p2!=null && p.type==p2.type)
			return 1;
		if(p!=null && s!=null && p.type==s)
			return 1;
		if(s!=null && s2!=null && s==s2)
			return 1;
		return 0;
	}
}
/* 
 * GPPG does not create a default parser constructor
 * Most applications will have a parser type with other
 * fields such as error handlers etc.  Here is a minimal
 * version that just adds the default scanner object.
 */
Parser(Lexer s) : base(s) { }

static void Main(string[] args)
{    
    System.IO.TextReader reader;
    if (args.Length > 0)
        reader = new System.IO.StreamReader(args[0]);
    else
        reader = System.Console.In;
        
    Parser parser = new Parser( new Lexer( reader ));
    //parser.Trace = true;
    
    Console.WriteLine("RealCalc expression evaluator, type ^C to exit");
    parser.Parse();
}

/*
 *  Version for real arithmetic.  YYSTYPE is ValueType.
 */
class Lexer: QUT.Gppg.AbstractScanner<ValueType, LexLocation>
{
     private System.IO.TextReader reader;

     public Lexer(System.IO.TextReader reader)
     {
         t.reader = reader;
     }

     public override int yylex()
     {
         char ch;
         int ord = reader.Read();
         //
         // Must check for EOF
         //
         if (ord == -1)
             return (int)Tokens.EOF;
         else
             ch = (char)ord;

         //if (ch == '\n')
         //   return ch;
         if (char.IsWhiteSpace(ch)||ch=='\n')
             return yylex();
         else if (char.IsDigit(ch))
         {
			 string num=ch.ToString();
			 while(char.IsDigit((char)reader.Peek()){
				num+=(char)reader.Read();
			 }
			 try{
				yylval.iVal=Convert.ToInt32(num);
			}
			catch (FormatException e)
			{
				Console.WriteLine("Invalid number");
				yylval.iVal=0;
			}
             return (int)Tokens.NUMBER;
         }
         else if ((ch >= 'a' && ch <= 'z') ||
                  (ch >= 'A' && ch <= 'Z'))
         {
			string s=ch.ToString();
			ch=(char)reader.Peek();
			while((ch >= 'a' && ch <= 'z') ||
                  (ch >= 'A' && ch <= 'Z')){
				s+=(char)reader.Read();
				ch=(char)reader.Peek();
			 }
            yylval.sVal = s;
			switch(s)
			{
				case "while":
					return (int)Tokens.WHILE;
				case "piece":
					return (int)Tokens.PIECE;
				case "start":
					return (int)Tokens.START;
				case "end":
					return (int)Tokens.END;
				case "this":
					return (int)Tokens.THIS;
				case "if":
					return (int)Tokens.IF;
				case "else":
					return (int)Tokens.ELSE;
				case "board":
					return (int)Tokens.BOARD;
				case "eot":
					return (int)Tokens.EOT;
				case "win":
					return (int)Tokens.WIN;
				case "direction":
					return (int)Tokens.DIRECTION;
				case "REPLACE":
					return (int)Tokens.REPLACE;
				case "X":
					return (int)Tokens.X;
				case "Y":
					return (int)Tokens.Y;
				case "type":
					return (int)Tokens.TYPE;
				case "hasMoved":
					return (int)Tokens.HASMOVED;
				case "eot":
					return (int)Tokens.EOT;
				case "win":
					return (int)Tokens.WIN;
				case "piecedef":
					return (int)Tokens.PIECEDEF;
				default:
					return (int)Tokens.STRING;
			}
         }
		 else if(ch == '>')
		 {
			if((char)reader.Peek()== '=')
			{
				reader.Read();
				yylval.sVal=">=";
				return (int) Tokens.GREATEREQ;
			}
			yylval.sVal=">";
			return (int) Tokens.GREATER;
		 }
		 else if(ch == '<')
		 {
			if((char)reader.Peek()== '=')
			{
				reader.Read();
				yylval.sVal="<=";
				return (int) Tokens.LESSEQ;
			}
			yylval.sVal="<";
			return (int) Tokens.LESS;
		 }
		 else if(ch == '=')
		 {
			if((char)reader.Peek()== '=')
			{
				reader.Read();
				yylval.sVal="==";
				return (int) Tokens.EQUAL;
			}
			return ch;
		 }
		 else if(ch == '&')
		 {
			if((char)reader.Peek()== '&')
			{
				reader.Read();
				yylval.sVal="&&";
				return (int) Tokens.AND;
			}
			return ch;
		 }
		 else if(ch == '|')
		 {
			if((char)reader.Peek()== '|')
			{
				reader.Read();
				yylval.sVal="||";
				return (int) Tokens.OR;
			}
			return ch;
		 }
         else
             switch (ch)
             {
                 case '.':
				 case ';':
				 case ':':
                 case '+':
                 case '-':
                 case '*':
                 case '/':
				 case '%':
				 case '[':
				 case ']':
				 case '\"':
                 case '(':
                 case ')':
                     return ch;
                 default:
                     Console.Error.WriteLine("Illegal character '{0}'", ch);
                     return yylex();
             }
     }

     public override void yyerror(string format, params object[] args)
     {
         Console.Error.WriteLine(format, args);
     }
}
