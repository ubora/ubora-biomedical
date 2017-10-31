import MarkdownRenderer from './modules/markdown_renderer';
import MarkdownEditor from './modules/markdown_editor';
import Autocomplete from './modules/autocomplete';
import CopyingToClipboard from './modules/clipboard';
import Feedback from './modules/feedback';
import Sidemenu from './modules/sidemenu';
import Notices from './modules/notices';
import DragNDropFileUploads from './modules/drag_n_drop_file_uploads';

window.addEventListener('load', () => {
  console.info(`UBORA: page loaded in ${Math.ceil(window.performance.now())}ms / target <500ms`);
});
