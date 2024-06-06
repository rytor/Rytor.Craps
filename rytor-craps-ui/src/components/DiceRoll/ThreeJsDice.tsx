import * as THREE from "three";
import { OrbitControls } from "three-orbitcontrols-ts";
import CANNON from "cannon";
import { DiceManager } from "threejs-dice";
import { DiceD6 } from "threejs-dice";
import { useEffect } from "react";
import { useAppDispatch } from "../../app/hooks";
import { rollEnd } from "../../features/game/game-slice";

const DEBUG_DICE_WALLS = false;

export default function ThreeJsDice({ targets }: { targets: number[] }) {
  let initialized = false;

  let requestAnimationFrameRef: number;

  // standard global variables
  let container: any;
  let scene: any;
  let camera: any;
  let renderer: any;
  let controls: any;
  let world: any;
  let dice: DiceD6[] = [];
  let barrier: any; // these are reused.. gotta fix that
  let wall: any; // these are reused.. gotta fix that

  // FUNCTIONS
  function init(totalDice: number) {
    // SCENE
    scene = new THREE.Scene();
    scene.backColor = null;
    // CAMERA
    var SCREEN_WIDTH = window.innerWidth,
      SCREEN_HEIGHT = window.innerHeight;
    var VIEW_ANGLE = 60,
      ASPECT = SCREEN_WIDTH / SCREEN_HEIGHT,
      NEAR = 0.01,
      FAR = 20000;
    camera = new THREE.PerspectiveCamera(VIEW_ANGLE, ASPECT, NEAR, FAR);
    scene.add(camera);
    camera.position.set(0, 30, 0);
    // RENDERER
    renderer = new THREE.WebGLRenderer({ antialias: true, alpha: true });
    renderer.setSize(SCREEN_WIDTH - 10, SCREEN_HEIGHT - 10);
    renderer.setClearColor(0x000000, 0); //default
    renderer.shadowMap.enabled = true;
    renderer.shadowMap.type = THREE.PCFSoftShadowMap;

    container = document.getElementById("ThreeJS");
    container!.appendChild(renderer.domElement);
    // EVENTS
    // CONTROLS
    controls = new OrbitControls(camera, renderer.domElement);
    controls.enabled = false;

    //   let ambient = new THREE.AmbientLight("#ffffff", 0.3);
    let hemiLight = new THREE.HemisphereLight(0x0000ff, 0x00ff00, 0.2);

    scene.add(hemiLight);

    let directionalLight = new THREE.DirectionalLight("#ffffff", 0.5);
    directionalLight.position.x = -1000;
    directionalLight.position.y = 1000;
    directionalLight.position.z = 1000;
    scene.add(directionalLight);

    let light = new THREE.SpotLight(0xefdfd5, 1.3);
    light.position.y = 100;
    light.target.position.set(0, 0, 0);
    light.castShadow = true;
    light.shadow.camera.near = 50;
    light.shadow.camera.far = 110;
    light.shadow.mapSize.width = 1024;
    light.shadow.mapSize.height = 1024;
    scene.add(light);

    ////////////
    // CUSTOM //
    ////////////
    world = new CANNON.World();

    world.gravity.set(0, -9.82 * 15, 0);
    world.broadphase = new CANNON.NaiveBroadphase();
    world.solver.iterations = 16;

    DiceManager.setWorld(world);

    //Floor
    let floorBody = new CANNON.Body({
      mass: 0,
      shape: new CANNON.Plane(),
      material: DiceManager.floorBodyMaterial,
    });
    floorBody.quaternion.setFromAxisAngle(
      new CANNON.Vec3(1, 0, 0),
      -Math.PI / 2
    );
    (world as any).add(floorBody);

    for (var i = 0; i < totalDice; i++) {
      var die = new DiceD6({
        size: 2,
        backColor: "#eee",
        fontColor: "#000",
      });
      scene.add(die.getObject());
      dice.push(die);
    }

    buildNorthWall();
    buildEastWall();
    buildSouthWall(); // the last 2 things added are weird until after first click
    buildWestWall();
  }

  function buildNorthWall() {
    barrier = new CANNON.Body({
      mass: 0,
      shape: new CANNON.Box(new CANNON.Vec3(75, 5, 1)),
    });
    barrier.position.set(0, 0, -15);
    world.addBody(barrier);
    wall = new THREE.Mesh(
      new THREE.BoxGeometry(75, 5, 1),
      new THREE.MeshPhongMaterial({
        color: "red",
        opacity: DEBUG_DICE_WALLS ? 1 : 0.0,
        transparent: !DEBUG_DICE_WALLS,
        side: THREE.DoubleSide,
      })
    );
    wall.quaternion.set(
      barrier.quaternion.x,
      barrier.quaternion.y,
      barrier.quaternion.z,
      barrier.quaternion.w
    );
    wall.position.set(
      barrier.position.x,
      barrier.position.y,
      barrier.position.z
    );
    scene.add(wall);
  }

  function buildWestWall() {
    barrier = new CANNON.Body({
      mass: 0,
      shape: new CANNON.Box(new CANNON.Vec3(1, 50, 50)),
    });
    barrier.position.set(-12 * (window.innerWidth / window.innerHeight), 0, 0);
    world.addBody(barrier);

    wall = new THREE.Mesh(
      new THREE.BoxGeometry(1, 10, 50),
      new THREE.MeshPhongMaterial({
        color: "green",
        opacity: DEBUG_DICE_WALLS ? 1 : 0.0,
        transparent: !DEBUG_DICE_WALLS,
        side: THREE.DoubleSide,
      })
    );
    wall.quaternion.set(
      barrier.quaternion.x,
      barrier.quaternion.y,
      barrier.quaternion.z,
      barrier.quaternion.w
    );
    wall.position.set(
      barrier.position.x,
      barrier.position.y,
      barrier.position.z
    );
    scene.add(wall);
  }

  function buildEastWall() {
    barrier = new CANNON.Body({
      mass: 0,
      shape: new CANNON.Box(new CANNON.Vec3(1, 10, 50)),
    });
    barrier.position.set(12 * (window.innerWidth / window.innerHeight), 0, 0);
    world.addBody(barrier);

    wall = new THREE.Mesh(
      new THREE.BoxGeometry(1, 10, 50),
      new THREE.MeshPhongMaterial({
        color: "#0000aa",
        opacity: DEBUG_DICE_WALLS ? 1 : 0.0,
        transparent: !DEBUG_DICE_WALLS,
        side: THREE.DoubleSide,
      })
    );
    wall.quaternion.set(
      barrier.quaternion.x,
      barrier.quaternion.y,
      barrier.quaternion.z,
      barrier.quaternion.w
    );
    wall.position.set(
      barrier.position.x,
      barrier.position.y,
      barrier.position.z
    );
    scene.add(wall);
  }

  function buildSouthWall() {
    barrier = new CANNON.Body({
      mass: 0,
      shape: new CANNON.Box(new CANNON.Vec3(75, 5, 1)),
    });
    barrier.position.set(0, 0, 15);
    world.addBody(barrier);

    wall = new THREE.Mesh(
      new THREE.BoxGeometry(75, 5, 1),
      new THREE.MeshPhongMaterial({
        color: "yellow",
        opacity: DEBUG_DICE_WALLS ? 1 : 0.0,
        transparent: !DEBUG_DICE_WALLS,
        side: THREE.DoubleSide,
      })
    );
    wall.quaternion.set(
      barrier.quaternion.x,
      barrier.quaternion.y,
      barrier.quaternion.z,
      barrier.quaternion.w
    );
    wall.position.set(
      barrier.position.x,
      barrier.position.y,
      barrier.position.z
    );
    scene.add(wall);
  }

  function throwDice(targets: number[]) {
    console.log("throwing", targets);
    var diceValues: any[] = [];

    for (var i = 0; i < dice.length; i++) {
      dice[i].getObject().position.x = -10 - (i % 3) * 1.5;
      dice[i].getObject().position.y = 5 + Math.floor(i / 3) * 1.5;
      dice[i].getObject().position.z = -10 + (i % 3) * 1.5;
      dice[i].getObject().quaternion.x =
        ((Math.random() * 90 - 45) * Math.PI) / 180;
      dice[i].getObject().quaternion.z =
        ((Math.random() * 90 - 45) * Math.PI) / 180;
      dice[i].updateBodyFromMesh();

      const yRand = Math.random() * 20;
      const rand = Math.random() * 5;

      dice[i].getObject().body.velocity.set(25 + rand, 40 + yRand, 15 + rand);
      dice[i]
        .getObject()
        .body.angularVelocity.set(
          20 * Math.random() - 10,
          50 * Math.random() - 10,
          20 * Math.random() - 10
        );

      diceValues.push({ dice: dice[i], value: targets[i] });
    }

    DiceManager.prepareValues(diceValues);
  }

  function animate() {
    updatePhysics();
    renderer.render(scene, camera);
    requestAnimationFrameRef = requestAnimationFrame(animate);
  }

  function updatePhysics() {
    world.step(1.0 / 60.0);

    for (var i in dice) {
      dice[i].updateMeshFromBody();
    }
  }

  const dispatch = useAppDispatch();

  useEffect(() => {
    // if (initialized) return;
    // initialized = true;

    if (requestAnimationFrameRef)
      cancelAnimationFrame(requestAnimationFrameRef);
    init(targets.length);
    requestAnimationFrameRef = requestAnimationFrame(animate);

    throwDice(targets);

    setTimeout(() => {
      dispatch(rollEnd());
    }, 5000);

    return () => {
      // empty(this.modelContainer);
      cancelAnimationFrame(requestAnimationFrameRef);
      cancelAnimationFrame(scene.id);
      scene = null;
      // projector = null;
      camera = null;
      controls = null;
    };
  }, []);

  // function handleClick() {
  //   throwDice(targets);
  // }

  return (
    <div
      id="ThreeJS"
      className="absolute z-1 pointer-events-none"
      style={{ position: "absolute", left: "0px", top: "0px" }}
    ></div>
  );
}
