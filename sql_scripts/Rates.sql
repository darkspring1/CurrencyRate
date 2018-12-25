DROP TABLE public."Rates";

CREATE TABLE public."Rates" (
	"Id" uuid NOT NULL,
	"Code" text NOT NULL,
	"Value" numeric(18, 2) NOT null,
	"Amount" int NOT null,
	"Date" date NOT NULL
)
WITH (
	OIDS=FALSE
) ;

CREATE UNIQUE INDEX date_code_idx ON public."Rates" ("Date", "Code");