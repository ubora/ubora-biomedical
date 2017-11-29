import MarkdownRenderer from './modules/markdown_renderer';
import MarkdownEditor from './modules/markdown_editor';
import Autocomplete from './modules/autocomplete';
import CopyingToClipboard from './modules/clipboard';
import Feedback from './modules/feedback';

window.addEventListener('load', () => {
  console.info(`UBORA: page loaded in ${Math.ceil(window.performance.now())}ms / target <500ms`);
});
