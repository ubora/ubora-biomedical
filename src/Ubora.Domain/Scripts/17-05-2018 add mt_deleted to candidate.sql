DO LANGUAGE plpgsql $tran$
BEGIN

-- Marten.Storage.DocumentTable
alter table public.mt_doc_candidate add column mt_deleted boolean DEFAULT FALSE;


-- Marten.Storage.DocumentTable
alter table public.mt_doc_candidate add column mt_deleted_at timestamp with time zone NULL;


-- Marten.Storage.DocumentTable
CREATE INDEX mt_doc_candidate_idx_mt_deleted ON public.mt_doc_candidate ("mt_deleted");




END;
$tran$;

