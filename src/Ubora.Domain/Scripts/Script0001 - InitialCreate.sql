DO LANGUAGE plpgsql $tran$
BEGIN

-- system_functions

CREATE OR REPLACE FUNCTION public.mt_immutable_timestamp(value text) RETURNS timestamp with time zone LANGUAGE sql IMMUTABLE AS $function$
    select value::timestamptz
$function$;

-- inotification

DROP TABLE IF EXISTS public.mt_doc_inotification CASCADE;
CREATE TABLE public.mt_doc_inotification (
    id                  uuid CONSTRAINT pk_mt_doc_inotification PRIMARY KEY,
    data                jsonb NOT NULL,
    mt_last_modified    timestamp with time zone DEFAULT transaction_timestamp(),
    mt_version          uuid NOT NULL default(md5(random()::text || clock_timestamp()::text)::uuid),
    mt_dotnet_type      varchar ,
    mt_doc_type         varchar DEFAULT 'BASE'
);
COMMENT ON TABLE public.mt_doc_inotification IS 'origin:Marten.IDocumentStore, Marten, Version=2.0.0.51134, Culture=neutral, PublicKeyToken=null';

CREATE INDEX mt_doc_inotification_idx_mt_doc_type ON public.mt_doc_inotification ("mt_doc_type");

CREATE OR REPLACE FUNCTION public.mt_upsert_inotification(doc JSONB, docDotNetType varchar, docId uuid, docType varchar, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
DECLARE
  final_version uuid;
BEGIN
INSERT INTO public.mt_doc_inotification ("data", "mt_dotnet_type", "id", "mt_doc_type", "mt_version", mt_last_modified) VALUES (doc, docDotNetType, docId, docType, docVersion, transaction_timestamp())
  ON CONFLICT ON CONSTRAINT pk_mt_doc_inotification
  DO UPDATE SET "data" = doc, "mt_dotnet_type" = docDotNetType, "mt_doc_type" = docType, "mt_version" = docVersion, mt_last_modified = transaction_timestamp();

  SELECT mt_version FROM public.mt_doc_inotification into final_version WHERE id = docId;
  RETURN final_version;
END;
$function$;


CREATE OR REPLACE FUNCTION public.mt_insert_inotification(doc JSONB, docDotNetType varchar, docId uuid, docType varchar, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
BEGIN
INSERT INTO public.mt_doc_inotification ("data", "mt_dotnet_type", "id", "mt_doc_type", "mt_version", mt_last_modified) VALUES (doc, docDotNetType, docId, docType, docVersion, transaction_timestamp());

  RETURN docVersion;
END;
$function$;


CREATE OR REPLACE FUNCTION public.mt_update_inotification(doc JSONB, docDotNetType varchar, docId uuid, docType varchar, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
DECLARE
  final_version uuid;
BEGIN
  UPDATE public.mt_doc_inotification SET "data" = doc, "mt_dotnet_type" = docDotNetType, "mt_doc_type" = docType, "mt_version" = docVersion, mt_last_modified = transaction_timestamp() where id = docId;

  SELECT mt_version FROM public.mt_doc_inotification into final_version WHERE id = docId;
  RETURN final_version;
END;
$function$;

-- project

DROP TABLE IF EXISTS public.mt_doc_project CASCADE;
CREATE TABLE public.mt_doc_project (
    id                  uuid CONSTRAINT pk_mt_doc_project PRIMARY KEY,
    data                jsonb NOT NULL,
    mt_last_modified    timestamp with time zone DEFAULT transaction_timestamp(),
    mt_version          uuid NOT NULL default(md5(random()::text || clock_timestamp()::text)::uuid),
    mt_dotnet_type      varchar 
);
COMMENT ON TABLE public.mt_doc_project IS 'origin:Marten.IDocumentStore, Marten, Version=2.0.0.51134, Culture=neutral, PublicKeyToken=null';

CREATE OR REPLACE FUNCTION public.mt_upsert_project(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
DECLARE
  final_version uuid;
BEGIN
INSERT INTO public.mt_doc_project ("data", "mt_dotnet_type", "id", "mt_version", mt_last_modified) VALUES (doc, docDotNetType, docId, docVersion, transaction_timestamp())
  ON CONFLICT ON CONSTRAINT pk_mt_doc_project
  DO UPDATE SET "data" = doc, "mt_dotnet_type" = docDotNetType, "mt_version" = docVersion, mt_last_modified = transaction_timestamp();

  SELECT mt_version FROM public.mt_doc_project into final_version WHERE id = docId;
  RETURN final_version;
END;
$function$;


CREATE OR REPLACE FUNCTION public.mt_insert_project(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
BEGIN
INSERT INTO public.mt_doc_project ("data", "mt_dotnet_type", "id", "mt_version", mt_last_modified) VALUES (doc, docDotNetType, docId, docVersion, transaction_timestamp());

  RETURN docVersion;
END;
$function$;


CREATE OR REPLACE FUNCTION public.mt_update_project(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
DECLARE
  final_version uuid;
BEGIN
  UPDATE public.mt_doc_project SET "data" = doc, "mt_dotnet_type" = docDotNetType, "mt_version" = docVersion, mt_last_modified = transaction_timestamp() where id = docId;

  SELECT mt_version FROM public.mt_doc_project into final_version WHERE id = docId;
  RETURN final_version;
END;
$function$;


-- userprofile

DROP TABLE IF EXISTS public.mt_doc_userprofile CASCADE;
CREATE TABLE public.mt_doc_userprofile (
    id                  uuid CONSTRAINT pk_mt_doc_userprofile PRIMARY KEY,
    data                jsonb NOT NULL,
    mt_last_modified    timestamp with time zone DEFAULT transaction_timestamp(),
    mt_version          uuid NOT NULL default(md5(random()::text || clock_timestamp()::text)::uuid),
    mt_dotnet_type      varchar 
);
COMMENT ON TABLE public.mt_doc_userprofile IS 'origin:Marten.IDocumentStore, Marten, Version=2.0.0.51134, Culture=neutral, PublicKeyToken=null';

CREATE OR REPLACE FUNCTION public.mt_upsert_userprofile(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
DECLARE
  final_version uuid;
BEGIN
INSERT INTO public.mt_doc_userprofile ("data", "mt_dotnet_type", "id", "mt_version", mt_last_modified) VALUES (doc, docDotNetType, docId, docVersion, transaction_timestamp())
  ON CONFLICT ON CONSTRAINT pk_mt_doc_userprofile
  DO UPDATE SET "data" = doc, "mt_dotnet_type" = docDotNetType, "mt_version" = docVersion, mt_last_modified = transaction_timestamp();

  SELECT mt_version FROM public.mt_doc_userprofile into final_version WHERE id = docId;
  RETURN final_version;
END;
$function$;


CREATE OR REPLACE FUNCTION public.mt_insert_userprofile(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
BEGIN
INSERT INTO public.mt_doc_userprofile ("data", "mt_dotnet_type", "id", "mt_version", mt_last_modified) VALUES (doc, docDotNetType, docId, docVersion, transaction_timestamp());

  RETURN docVersion;
END;
$function$;


CREATE OR REPLACE FUNCTION public.mt_update_userprofile(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
DECLARE
  final_version uuid;
BEGIN
  UPDATE public.mt_doc_userprofile SET "data" = doc, "mt_dotnet_type" = docDotNetType, "mt_version" = docVersion, mt_last_modified = transaction_timestamp() where id = docId;

  SELECT mt_version FROM public.mt_doc_userprofile into final_version WHERE id = docId;
  RETURN final_version;
END;
$function$;


-- workpackageone

DROP TABLE IF EXISTS public.mt_doc_workpackageone CASCADE;
CREATE TABLE public.mt_doc_workpackageone (
    id                  uuid CONSTRAINT pk_mt_doc_workpackageone PRIMARY KEY,
    data                jsonb NOT NULL,
    mt_last_modified    timestamp with time zone DEFAULT transaction_timestamp(),
    mt_version          uuid NOT NULL default(md5(random()::text || clock_timestamp()::text)::uuid),
    mt_dotnet_type      varchar 
);
COMMENT ON TABLE public.mt_doc_workpackageone IS 'origin:Marten.IDocumentStore, Marten, Version=2.0.0.51134, Culture=neutral, PublicKeyToken=null';

CREATE OR REPLACE FUNCTION public.mt_upsert_workpackageone(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
DECLARE
  final_version uuid;
BEGIN
INSERT INTO public.mt_doc_workpackageone ("data", "mt_dotnet_type", "id", "mt_version", mt_last_modified) VALUES (doc, docDotNetType, docId, docVersion, transaction_timestamp())
  ON CONFLICT ON CONSTRAINT pk_mt_doc_workpackageone
  DO UPDATE SET "data" = doc, "mt_dotnet_type" = docDotNetType, "mt_version" = docVersion, mt_last_modified = transaction_timestamp();

  SELECT mt_version FROM public.mt_doc_workpackageone into final_version WHERE id = docId;
  RETURN final_version;
END;
$function$;


CREATE OR REPLACE FUNCTION public.mt_insert_workpackageone(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
BEGIN
INSERT INTO public.mt_doc_workpackageone ("data", "mt_dotnet_type", "id", "mt_version", mt_last_modified) VALUES (doc, docDotNetType, docId, docVersion, transaction_timestamp());

  RETURN docVersion;
END;
$function$;


CREATE OR REPLACE FUNCTION public.mt_update_workpackageone(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
DECLARE
  final_version uuid;
BEGIN
  UPDATE public.mt_doc_workpackageone SET "data" = doc, "mt_dotnet_type" = docDotNetType, "mt_version" = docVersion, mt_last_modified = transaction_timestamp() where id = docId;

  SELECT mt_version FROM public.mt_doc_workpackageone into final_version WHERE id = docId;
  RETURN final_version;
END;
$function$;

-- workpackagethree

DROP TABLE IF EXISTS public.mt_doc_workpackagethree CASCADE;
CREATE TABLE public.mt_doc_workpackagethree (
    id                  uuid CONSTRAINT pk_mt_doc_workpackagethree PRIMARY KEY,
    data                jsonb NOT NULL,
    mt_last_modified    timestamp with time zone DEFAULT transaction_timestamp(),
    mt_version          uuid NOT NULL default(md5(random()::text || clock_timestamp()::text)::uuid),
    mt_dotnet_type      varchar 
);
COMMENT ON TABLE public.mt_doc_workpackagethree IS 'origin:Marten.IDocumentStore, Marten, Version=2.0.0.51134, Culture=neutral, PublicKeyToken=null';

CREATE OR REPLACE FUNCTION public.mt_upsert_workpackagethree(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
DECLARE
  final_version uuid;
BEGIN
INSERT INTO public.mt_doc_workpackagethree ("data", "mt_dotnet_type", "id", "mt_version", mt_last_modified) VALUES (doc, docDotNetType, docId, docVersion, transaction_timestamp())
  ON CONFLICT ON CONSTRAINT pk_mt_doc_workpackagethree
  DO UPDATE SET "data" = doc, "mt_dotnet_type" = docDotNetType, "mt_version" = docVersion, mt_last_modified = transaction_timestamp();

  SELECT mt_version FROM public.mt_doc_workpackagethree into final_version WHERE id = docId;
  RETURN final_version;
END;
$function$;


CREATE OR REPLACE FUNCTION public.mt_insert_workpackagethree(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
BEGIN
INSERT INTO public.mt_doc_workpackagethree ("data", "mt_dotnet_type", "id", "mt_version", mt_last_modified) VALUES (doc, docDotNetType, docId, docVersion, transaction_timestamp());

  RETURN docVersion;
END;
$function$;


CREATE OR REPLACE FUNCTION public.mt_update_workpackagethree(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
DECLARE
  final_version uuid;
BEGIN
  UPDATE public.mt_doc_workpackagethree SET "data" = doc, "mt_dotnet_type" = docDotNetType, "mt_version" = docVersion, mt_last_modified = transaction_timestamp() where id = docId;

  SELECT mt_version FROM public.mt_doc_workpackagethree into final_version WHERE id = docId;
  RETURN final_version;
END;
$function$;


-- workpackagetwo

DROP TABLE IF EXISTS public.mt_doc_workpackagetwo CASCADE;
CREATE TABLE public.mt_doc_workpackagetwo (
    id                  uuid CONSTRAINT pk_mt_doc_workpackagetwo PRIMARY KEY,
    data                jsonb NOT NULL,
    mt_last_modified    timestamp with time zone DEFAULT transaction_timestamp(),
    mt_version          uuid NOT NULL default(md5(random()::text || clock_timestamp()::text)::uuid),
    mt_dotnet_type      varchar 
);
COMMENT ON TABLE public.mt_doc_workpackagetwo IS 'origin:Marten.IDocumentStore, Marten, Version=2.0.0.51134, Culture=neutral, PublicKeyToken=null';

CREATE OR REPLACE FUNCTION public.mt_upsert_workpackagetwo(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
DECLARE
  final_version uuid;
BEGIN
INSERT INTO public.mt_doc_workpackagetwo ("data", "mt_dotnet_type", "id", "mt_version", mt_last_modified) VALUES (doc, docDotNetType, docId, docVersion, transaction_timestamp())
  ON CONFLICT ON CONSTRAINT pk_mt_doc_workpackagetwo
  DO UPDATE SET "data" = doc, "mt_dotnet_type" = docDotNetType, "mt_version" = docVersion, mt_last_modified = transaction_timestamp();

  SELECT mt_version FROM public.mt_doc_workpackagetwo into final_version WHERE id = docId;
  RETURN final_version;
END;
$function$;


CREATE OR REPLACE FUNCTION public.mt_insert_workpackagetwo(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
BEGIN
INSERT INTO public.mt_doc_workpackagetwo ("data", "mt_dotnet_type", "id", "mt_version", mt_last_modified) VALUES (doc, docDotNetType, docId, docVersion, transaction_timestamp());

  RETURN docVersion;
END;
$function$;


CREATE OR REPLACE FUNCTION public.mt_update_workpackagetwo(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
DECLARE
  final_version uuid;
BEGIN
  UPDATE public.mt_doc_workpackagetwo SET "data" = doc, "mt_dotnet_type" = docDotNetType, "mt_version" = docVersion, mt_last_modified = transaction_timestamp() where id = docId;

  SELECT mt_version FROM public.mt_doc_workpackagetwo into final_version WHERE id = docId;
  RETURN final_version;
END;
$function$;

-- deviceclassification

DROP TABLE IF EXISTS public.mt_doc_deviceclassification CASCADE;
CREATE TABLE public.mt_doc_deviceclassification (
    id                  uuid CONSTRAINT pk_mt_doc_deviceclassification PRIMARY KEY,
    data                jsonb NOT NULL,
    mt_last_modified    timestamp with time zone DEFAULT transaction_timestamp(),
    mt_version          uuid NOT NULL default(md5(random()::text || clock_timestamp()::text)::uuid),
    mt_dotnet_type      varchar 
);
COMMENT ON TABLE public.mt_doc_deviceclassification IS 'origin:Marten.IDocumentStore, Marten, Version=2.0.0.51134, Culture=neutral, PublicKeyToken=null';

CREATE OR REPLACE FUNCTION public.mt_upsert_deviceclassification(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
DECLARE
  final_version uuid;
BEGIN
INSERT INTO public.mt_doc_deviceclassification ("data", "mt_dotnet_type", "id", "mt_version", mt_last_modified) VALUES (doc, docDotNetType, docId, docVersion, transaction_timestamp())
  ON CONFLICT ON CONSTRAINT pk_mt_doc_deviceclassification
  DO UPDATE SET "data" = doc, "mt_dotnet_type" = docDotNetType, "mt_version" = docVersion, mt_last_modified = transaction_timestamp();

  SELECT mt_version FROM public.mt_doc_deviceclassification into final_version WHERE id = docId;
  RETURN final_version;
END;
$function$;


CREATE OR REPLACE FUNCTION public.mt_insert_deviceclassification(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
BEGIN
INSERT INTO public.mt_doc_deviceclassification ("data", "mt_dotnet_type", "id", "mt_version", mt_last_modified) VALUES (doc, docDotNetType, docId, docVersion, transaction_timestamp());

  RETURN docVersion;
END;
$function$;


CREATE OR REPLACE FUNCTION public.mt_update_deviceclassification(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
DECLARE
  final_version uuid;
BEGIN
  UPDATE public.mt_doc_deviceclassification SET "data" = doc, "mt_dotnet_type" = docDotNetType, "mt_version" = docVersion, mt_last_modified = transaction_timestamp() where id = docId;

  SELECT mt_version FROM public.mt_doc_deviceclassification into final_version WHERE id = docId;
  RETURN final_version;
END;
$function$;

-- projectfile

DROP TABLE IF EXISTS public.mt_doc_projectfile CASCADE;
CREATE TABLE public.mt_doc_projectfile (
    id                  uuid CONSTRAINT pk_mt_doc_projectfile PRIMARY KEY,
    data                jsonb NOT NULL,
    mt_last_modified    timestamp with time zone DEFAULT transaction_timestamp(),
    mt_version          uuid NOT NULL default(md5(random()::text || clock_timestamp()::text)::uuid),
    mt_dotnet_type      varchar 
);
COMMENT ON TABLE public.mt_doc_projectfile IS 'origin:Marten.IDocumentStore, Marten, Version=2.0.0.51134, Culture=neutral, PublicKeyToken=null';

CREATE OR REPLACE FUNCTION public.mt_upsert_projectfile(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
DECLARE
  final_version uuid;
BEGIN
INSERT INTO public.mt_doc_projectfile ("data", "mt_dotnet_type", "id", "mt_version", mt_last_modified) VALUES (doc, docDotNetType, docId, docVersion, transaction_timestamp())
  ON CONFLICT ON CONSTRAINT pk_mt_doc_projectfile
  DO UPDATE SET "data" = doc, "mt_dotnet_type" = docDotNetType, "mt_version" = docVersion, mt_last_modified = transaction_timestamp();

  SELECT mt_version FROM public.mt_doc_projectfile into final_version WHERE id = docId;
  RETURN final_version;
END;
$function$;


CREATE OR REPLACE FUNCTION public.mt_insert_projectfile(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
BEGIN
INSERT INTO public.mt_doc_projectfile ("data", "mt_dotnet_type", "id", "mt_version", mt_last_modified) VALUES (doc, docDotNetType, docId, docVersion, transaction_timestamp());

  RETURN docVersion;
END;
$function$;


CREATE OR REPLACE FUNCTION public.mt_update_projectfile(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
DECLARE
  final_version uuid;
BEGIN
  UPDATE public.mt_doc_projectfile SET "data" = doc, "mt_dotnet_type" = docDotNetType, "mt_version" = docVersion, mt_last_modified = transaction_timestamp() where id = docId;

  SELECT mt_version FROM public.mt_doc_projectfile into final_version WHERE id = docId;
  RETURN final_version;
END;
$function$;

-- projecttask

DROP TABLE IF EXISTS public.mt_doc_projecttask CASCADE;
CREATE TABLE public.mt_doc_projecttask (
    id                  uuid CONSTRAINT pk_mt_doc_projecttask PRIMARY KEY,
    data                jsonb NOT NULL,
    mt_last_modified    timestamp with time zone DEFAULT transaction_timestamp(),
    mt_version          uuid NOT NULL default(md5(random()::text || clock_timestamp()::text)::uuid),
    mt_dotnet_type      varchar 
);
COMMENT ON TABLE public.mt_doc_projecttask IS 'origin:Marten.IDocumentStore, Marten, Version=2.0.0.51134, Culture=neutral, PublicKeyToken=null';

CREATE OR REPLACE FUNCTION public.mt_upsert_projecttask(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
DECLARE
  final_version uuid;
BEGIN
INSERT INTO public.mt_doc_projecttask ("data", "mt_dotnet_type", "id", "mt_version", mt_last_modified) VALUES (doc, docDotNetType, docId, docVersion, transaction_timestamp())
  ON CONFLICT ON CONSTRAINT pk_mt_doc_projecttask
  DO UPDATE SET "data" = doc, "mt_dotnet_type" = docDotNetType, "mt_version" = docVersion, mt_last_modified = transaction_timestamp();

  SELECT mt_version FROM public.mt_doc_projecttask into final_version WHERE id = docId;
  RETURN final_version;
END;
$function$;


CREATE OR REPLACE FUNCTION public.mt_insert_projecttask(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
BEGIN
INSERT INTO public.mt_doc_projecttask ("data", "mt_dotnet_type", "id", "mt_version", mt_last_modified) VALUES (doc, docDotNetType, docId, docVersion, transaction_timestamp());

  RETURN docVersion;
END;
$function$;


CREATE OR REPLACE FUNCTION public.mt_update_projecttask(doc JSONB, docDotNetType varchar, docId uuid, docVersion uuid) RETURNS UUID LANGUAGE plpgsql SECURITY INVOKER AS $function$
DECLARE
  final_version uuid;
BEGIN
  UPDATE public.mt_doc_projecttask SET "data" = doc, "mt_dotnet_type" = docDotNetType, "mt_version" = docVersion, mt_last_modified = transaction_timestamp() where id = docId;

  SELECT mt_version FROM public.mt_doc_projecttask into final_version WHERE id = docId;
  RETURN final_version;
END;
$function$;

-- transforms

-- Now can't use the JavaScript functions and you need to install http://jasperfx.github.io/marten/documentation/admin/installing-plv8-windows/


-- eventstore

DROP TABLE IF EXISTS public.mt_streams CASCADE;
CREATE TABLE public.mt_streams (
    id                  uuid CONSTRAINT pk_mt_streams PRIMARY KEY,
    type                varchar NULL,
    version             integer NOT NULL,
    timestamp           timestamptz default (now()) NOT NULL,
    snapshot            jsonb ,
    snapshot_version    integer ,
    created             timestamptz default (now()) NOT NULL,
    tenant_id           varchar DEFAULT '*DEFAULT*'
);
COMMENT ON TABLE public.mt_streams IS 'origin:Marten.IDocumentStore, Marten, Version=2.0.0.51134, Culture=neutral, PublicKeyToken=null';
DROP TABLE IF EXISTS public.mt_events CASCADE;
CREATE TABLE public.mt_events (
    seq_id            bigint CONSTRAINT pk_mt_events PRIMARY KEY,
    id                uuid NOT NULL,
    stream_id         uuid REFERENCES public.mt_streams ON DELETE CASCADE,
    version           integer NOT NULL,
    data              jsonb NOT NULL,
    type              varchar(100) NOT NULL,
    timestamp         timestamptz default (now()) NOT NULL,
    tenant_id         varchar DEFAULT '*DEFAULT*',
    mt_dotnet_type    varchar NULL,
    CONSTRAINT pk_mt_events_stream_and_version UNIQUE(stream_id, version),
    CONSTRAINT pk_mt_events_id_unique UNIQUE(id)
);
COMMENT ON TABLE public.mt_events IS 'origin:Marten.IDocumentStore, Marten, Version=2.0.0.51134, Culture=neutral, PublicKeyToken=null';
DROP TABLE IF EXISTS public.mt_event_progression CASCADE;
CREATE TABLE public.mt_event_progression (
    name           varchar CONSTRAINT pk_mt_event_progression PRIMARY KEY,
    last_seq_id    bigint NULL
);
COMMENT ON TABLE public.mt_event_progression IS 'origin:Marten.IDocumentStore, Marten, Version=2.0.0.51134, Culture=neutral, PublicKeyToken=null';
CREATE SEQUENCE public.mt_events_sequence;
ALTER SEQUENCE public.mt_events_sequence OWNED BY public.mt_events.seq_id;

CREATE OR REPLACE FUNCTION public.mt_append_event(stream uuid, stream_type varchar, tenantid varchar, event_ids uuid[], event_types varchar[], dotnet_types varchar[], bodies jsonb[]) RETURNS int[] AS $$
DECLARE
	event_version int;
	event_type varchar;
	event_id uuid;
	body jsonb;
	index int;
	seq int;
    actual_tenant varchar;
	return_value int[];
BEGIN
	select version into event_version from public.mt_streams where id = stream;
	if event_version IS NULL then
		event_version = 0;
		insert into public.mt_streams (id, type, version, timestamp, tenant_id) values (stream, stream_type, 0, now(), tenantid);
    else
        if tenantid IS NOT NULL then
            select tenant_id into actual_tenant from public.mt_streams where id = stream;
            if actual_tenant != tenantid then
                RAISE EXCEPTION 'The tenantid does not match the existing stream';
            end if;
        end if;
	end if;


	index := 1;
	return_value := ARRAY[event_version + array_length(event_ids, 1)];

	foreach event_id in ARRAY event_ids
	loop
	    seq := nextval('public.mt_events_sequence');
		return_value := array_append(return_value, seq);

	    event_version := event_version + 1;
		event_type = event_types[index];
		body = bodies[index];

		insert into public.mt_events 
			(seq_id, id, stream_id, version, data, type, tenant_id, mt_dotnet_type) 
		values 
			(seq, event_id, stream, event_version, body, event_type, tenantid, dotnet_types[index]);

		
		index := index + 1;
	end loop;

	update public.mt_streams set version = event_version, timestamp = now() where id = stream;

	return return_value;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION public.mt_mark_event_progression(name varchar, last_encountered bigint) RETURNS VOID LANGUAGE plpgsql AS $function$
BEGIN
INSERT INTO public.mt_event_progression (name, last_seq_id) VALUES (name, last_encountered)
  ON CONFLICT ON CONSTRAINT pk_mt_event_progression
  DO UPDATE SET last_seq_id = last_encountered;

END;
$function$;


END;
$tran$;