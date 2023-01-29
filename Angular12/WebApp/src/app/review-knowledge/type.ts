import { type } from "os";

interface User { FirstName: string; LastName: string; }

type TestOmit = Omit<User, "FirstName">;
const avc: TestOmit[] = [{ LastName : "ASDASDAS"}];
console.log(avc[0].LastName);

type TestNullable = NonNullable<User[]>;
type TestPartial = Partial<User[]>;
type TestReadOnly = Readonly<User[]>;
const abc: TestNullable = [
  { FirstName: "Hai123", LastName: "Hai123"  },
  { FirstName: "Hai456", LastName: "Hai456"  }
];
