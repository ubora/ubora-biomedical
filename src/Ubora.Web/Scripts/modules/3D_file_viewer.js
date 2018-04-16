export class ThreeDimensionalFileViewer {
    static initialize() {
        var camera, controls, scene, renderer;
        var lighting, ambient, keyLight, fillLight, backLight;
        init();
        animate();
        function init() {
            /* Camera */
            camera = new THREE.PerspectiveCamera(45, window.innerWidth / window.innerHeight, 1, 1000);
            camera.position.z = 50;

            /* Scene */
            scene = new THREE.Scene();
            lighting = false;
            ambient = new THREE.AmbientLight(0xffffff, 1.0);
            ambient.intensity = 0.25;
            scene.add(ambient);;
            keyLight = new THREE.DirectionalLight(new THREE.Color('hsl(30, 100%, 75%)'), 1.0);
            keyLight.position.set(-100, 0, 100);
            fillLight = new THREE.DirectionalLight(new THREE.Color('hsl(240, 100%, 75%)'), 0.75);
            fillLight.position.set(100, 0, 100);
            backLight = new THREE.DirectionalLight(0xffffff, 1.0);
            backLight.position.set(100, 0, -100).normalize();
            scene.add(keyLight);
            scene.add(fillLight);
            scene.add(backLight);

            /* Model */
            var file = "https://raw.githubusercontent.com/takahirox/takahirox.github.io/master/three.js.mmdeditor/examples/models/amf/rook.amf";

            /* Renderer */
            renderer = new THREE.WebGLRenderer();
            renderer.setPixelRatio(window.devicePixelRatio);
            renderer.setSize(600, 600);
            renderer.setClearColor(new THREE.Color("hsl(0, 0%, 10%)"));
            var container = document.getElementById('container');
            container.appendChild(renderer.domElement);

            var isLoaded3Dfile = load3Dfile(file);

            if (!isLoaded3Dfile) {
                console.log("Can't load the file!");
            }

            /* Controls */
            controls = new THREE.OrbitControls(camera, renderer.domElement);
            controls.enableDamping = true;
            controls.dampingFactor = 0.25;
            controls.enableZoom = false;

            /* Events */
            window.addEventListener('keydown', onKeyboardEvent, false);
        }

        function onKeyboardEvent(e) {
            if (e.code === 'KeyZ') {
                camera.fov = camera.fov + 1;
                camera.updateProjectionMatrix();

            } else if (e.code === 'KeyX') {
                camera.fov = camera.fov - 1;
                camera.updateProjectionMatrix();
            }
        }

        function animate() {
            requestAnimationFrame(animate);
            controls.update();
            render();
        }

        function render() {
            renderer.render(scene, camera);
        }

        function nexusRender() {
            Nexus.beginFrame(renderer.context);
            renderer.render(scene, camera);
            Nexus.endFrame(renderer.context);
        }

        function load3Dfile(filename) {
            var ext = getExtension(filename);
            switch (ext.toLowerCase()) {
                case 'stl':
                    var loader = new THREE.STLLoader();
                    loader.load(filename, function (geometry) {
                        var material = new THREE.MeshPhongMaterial();
                        var mesh = new THREE.Mesh(geometry, material);
                        scene.add(mesh);
                    });
                    return true;
                case 'amf':
                    var loader = new THREE.AMFLoader();
                    loader.load(filename, function (amfobject) {
                        scene.add(amfobject);
                    });
                    return true;
                case 'nxz':
                    var nexus_obj = new NexusObject(filename, renderer, nexusRender);
                    scene.add(nexus_obj);
                    return true;
            }
            return false;
        }

        function getExtension(filename) {
            var parts = filename.split('.');
            return parts[parts.length - 1];
        }
    }
}

ThreeDimensionalFileViewer.initialize();
console.log(window.init3Dviewer)