import mainImage from'./assets/images/craps_table.png';

//then in the render function of Jsx insert the mainLogo variable

// class CrapsTableImage extends Component {
//   render() {
//     return (
//       <nav className="nav" style={nbStyle}>
//         <div className="container">
//           //right below here
//           <img  src={mainImage} style={nbStyle.logo}/>
//         </div>
//       </nav>
//     );
//   }
// }

function CrapsTableImage() {
    return (
      <nav className="nav">
        <div className="container">
          <img  src={mainImage} />
        </div>
      </nav>
    );
  }

export default CrapsTableImage;