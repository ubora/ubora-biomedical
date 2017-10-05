-- Add ProjectId
update mt_events me set data = jsonb_set(data, '{ProjectId}', to_jsonb(t.id)) from (select ms.id from mt_streams ms ) t where me.stream_id = t.id;

-- Remove Id
update mt_events me set data = data - 'Id';