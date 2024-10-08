// Simple Derivative Calculator for CPSC 3400 at Seattle University
// Credit to Professor Beatriz Diaz Acosta for all things labeled "DO NOT MODIFY"
// Simplify function written by me


// Expression type (DO NOT MODIFY)
type Expression =
    | X
    | Y
    | Const of float
    | Neg of Expression
    | Add of Expression * Expression
    | Sub of Expression * Expression
    | Mul of Expression * Expression

// exprToString formatting function (DO NOT MODIFY)
let exprToString expr =
    let rec recExprStr parens expr =
        let lParen = if parens then "(" else ""
        let rParen = if parens then ")" else ""
        match expr with
        | X -> "x"
        | Y -> "y"
        | Const n -> n.ToString()
        | Neg e -> lParen + "-" + recExprStr true e + rParen
        | Add (e1, e2) -> lParen + recExprStr true e1 + "+" + recExprStr true e2 + rParen
        | Sub (e1, e2) -> lParen + recExprStr true e1 + "-" + recExprStr true e2 + rParen
        | Mul (e1, e2) -> lParen + recExprStr true e1 + "*" + recExprStr true e2 + rParen
    recExprStr false expr

// simplify function
let rec simplify (expr:Expression) = 
  match expr with
  | X -> X 
  | Y -> Y 
  | Const c -> Const c 
  //Add, simplification
  | Add(Const u, Const v) -> Const (u+v)
  | Add(u, Const 0.0) -> u 
  | Add(Const 0.0, v) -> v
  //Add, all other cases, look inward
  | Add(u, v) -> let a = simplify(u)
                 let b = simplify(v)
                 if a <> u || b <> v then simplify(Add(a,b)) else Add(a, b)
  //Sub, simplification
  | Sub(Const u, Const v) -> Const (u-v)
  | Sub(u, Const 0.0) -> u
  | Sub(Const 0.0, v) -> let a = simplify(v)
                         if a <> v then simplify(Neg(a)) else Neg(a)
  | Sub(u, v) -> let a = simplify(u)
                 let b = simplify(v)
                 if a=b then Const 0.0 
                 else if a <> u || b <> v then simplify(Sub(a,b))
                 else Sub(a, b) 
  //Mul, simplification
  | Mul(Const u, Const v) -> Const (u*v)
  | Mul(u, Const 0.0) -> Const 0.0
  | Mul(Const 0.0, v) -> Const 0.0
  | Mul(u, Const 1.0) -> u
  | Mul(Const 1.0, v) -> v 
  //Mul, all other cases
  | Mul(u, v) -> let a = simplify(u)
                 let b = simplify(v)
                 if a <> u || b <> v then simplify(Mul(a,b)) else Mul(a, b)
  //Neg, simplification
  | Neg(Const u) -> Const ((-1.0)*u)
  | Neg(Neg u) -> u 
  //Neg, all other cases
  | Neg(ex) -> let a = simplify(ex)
               if a <> ex then simplify(Neg(a)) else Neg(a) 
  | _ -> failwith(sprintf "Unable to match expression [%A]" expr)


// Provided Tests (DO NOT MODIFY)
printfn "---Provided Tests---"
let t1 = Add (Const 9.0, Const 4.0)
let t2 = Sub (Const 10.0, Const 3.5)
let t3 = Mul (Const 6.0, Const 7.0)
let t4 = Neg (Const 0.3)
let t5 = Neg (Const -9.0)
let t6 = Add (X, Const 0.0)
let t7 = Add (Const 0.0, Y)
let t8 = Sub (X, Const 0.0)
let t9 = Sub (Const 0.0, Y)
let t10 = Sub (Y, Y)
let t11 = Mul (X, Const 0.0)
let t12 = Mul (Const 0.0, Y)
let t13 = Mul (X, Const 1.0)
let t14 = Mul (Const 1.0, Y)
let t15 = Neg (Neg X)
let t16 = Sub (Mul (Const 1.0, X), Add (X, Const 0.0))
let t17 = Add (Sub (Const 3.0, Const 8.0), Mul (Const 7.0, Const 3.0))
let t18 = Sub (Sub (Add (Y, Const 3.0), Add (Y, Const 3.0)), Add (Const 0.0, Add (Y, Const 3.0)))
let t19 = Sub (Const 0.0, Neg (Mul (Const 1.0, X)))
let t20 = Mul (Add (X, Const 2.0), Neg (Sub (Mul (Const 2.0, Y), Const 5.0)))

printfn "t1  Correct: 13\t\tActual: %s" (exprToString (simplify t1))
printfn "t2  Correct: 6.5\tActual: %s" (exprToString (simplify t2)) 
printfn "t3  Correct: 42\t\tActual: %s" (exprToString (simplify t3))
printfn "t4  Correct: -0.3\tActual: %s" (exprToString (simplify t4))
printfn "t5  Correct: 9\t\tActual: %s" (exprToString (simplify t5))
printfn "t6  Correct: x\t\tActual: %s" (exprToString (simplify t6))
printfn "t7  Correct: y\t\tActual: %s" (exprToString (simplify t7))
printfn "t8  Correct: x\t\tActual: %s" (exprToString (simplify t8))
printfn "t9  Correct: -y\t\tActual: %s" (exprToString (simplify t9))
printfn "t10 Correct: 0\t\tActual: %s" (exprToString (simplify t10))
printfn "t11 Correct: 0\t\tActual: %s" (exprToString (simplify t11))
printfn "t12 Correct: 0\t\tActual: %s" (exprToString (simplify t12))
printfn "t13 Correct: x\t\tActual: %s" (exprToString (simplify t13))
printfn "t14 Correct: y\t\tActual: %s" (exprToString (simplify t14))
printfn "t15 Correct: x\t\tActual: %s" (exprToString (simplify t15))
printfn "t16 Correct: 0\t\tActual: %s" (exprToString (simplify t16))
printfn "t17 Correct: 16\t\tActual: %s" (exprToString (simplify t17)) 
printfn "t18 Correct: -(y+3)\tActual: %s" (exprToString (simplify t18))
printfn "t19 Correct: x\t\tActual: %s" (exprToString (simplify t19))
printfn "t20 Correct: (x+2)*(-((2*y)-5))"
printfn "    Actual:  %s" (exprToString (simplify t20))
