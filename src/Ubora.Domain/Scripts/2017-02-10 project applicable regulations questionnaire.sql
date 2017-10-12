DO LANGUAGE plpgsql $tran$
BEGIN

DROP TABLE IF EXISTS public.mt_doc_applicableregulationsquestionnaireaggregate CASCADE;
CREATE TABLE public.mt_doc_applicableregulationsquestionnaireaggregate (
    id                  uuid CONSTRAINT pk_mt_doc_applicableregulationsquestionnaireaggregate PRIMARY KEY,
    data                jsonb NOT NULL,
    mt_last_modified    timestamp with time zone DEFAULT transaction_timestamp(),
    mt_version          uuid NOT NULL default(md5(random()::text || clock_timestamp()::text)::uuid),
    mt_dotnet_type      varchar 
);
COMMENT ON TABLE public.mt_doc_applicableregulationsquestionnaireaggregate IS 'origin:Marten.IDocumentStore, Marten, Version=2.0.0.50850, Culture=neutral, PublicKeyToken=null';

CREATE OR REPLACE FUNCTION public.mt_upsert_applicableregulationsquestionnaireaggregate(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
DECLARE
  final_version uuid;
BEGIN
INSERT INTO public.mt_doc_applicableregulationsquestionnaireaggregate ("data", "mt_dotnet_type", "id", "mt_version", mt_last_modified) VALUES (doc, docDotNetType, docId, docVersion, transaction_timestamp())
  ON CONFLICT ON CONSTRAINT pk_mt_doc_applicableregulationsquestionnaireaggregate
  DO UPDATE SET "data" = doc, "mt_dotnet_type" = docDotNetType, "mt_version" = docVersion, mt_last_modified = transaction_timestamp();

  SELECT mt_version FROM public.mt_doc_applicableregulationsquestionnaireaggregate into final_version WHERE id = docId;
  RETURN final_version;
END;
$function$;


CREATE OR REPLACE FUNCTION public.mt_insert_applicableregulationsquestionnaireaggregate(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
BEGIN
INSERT INTO public.mt_doc_applicableregulationsquestionnaireaggregate ("data", "mt_dotnet_type", "id", "mt_version", mt_last_modified) VALUES (doc, docDotNetType, docId, docVersion, transaction_timestamp());

  RETURN docVersion;
END;
$function$;


CREATE OR REPLACE FUNCTION public.mt_update_applicableregulationsquestionnaireaggregate(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
DECLARE
  final_version uuid;
BEGIN
  UPDATE public.mt_doc_applicableregulationsquestionnaireaggregate SET "data" = doc, "mt_dotnet_type" = docDotNetType, "mt_version" = docVersion, mt_last_modified = transaction_timestamp() where id = docId;

  SELECT mt_version FROM public.mt_doc_applicableregulationsquestionnaireaggregate into final_version WHERE id = docId;
  RETURN final_version;
END;
$function$;



END;
$tran$;
