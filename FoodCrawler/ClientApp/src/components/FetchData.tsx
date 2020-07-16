import React, { Component } from 'react';
import {Icon} from 'semantic-ui-react';
import { makeStyles } from '@material-ui/core/styles';
import {Card, CardActionArea, CardMedia, CardContent, Typography, CardActions, Button, Grid, Divider, IconButton, Chip, Box} from '@material-ui/core';
import '../custom.scss';
import {ShoppingCart} from '@material-ui/icons'


interface IState {
  expandedRows: any[];
  loading : boolean;
  foodItems : any[];
}

interface Props {

}

export class FetchData extends React.PureComponent<Props,IState> {
  static displayName = FetchData.name;
  //state: IState = { loading: true, expandedRows: [], foodItems : []};

  constructor(props: Props) {
    super(props);        
    this.state = { expandedRows: [], loading: true, foodItems : [] };
  }

  componentDidMount() {
    //this.populateWeatherData();
    this.populateFoodData();    
  }

  singleCard(item: any) {
    //const classes = useStyles();
  
    return (
      <Grid container item xs={12} sm={6} md={4} style={{display:'flex'}} spacing={3} >
      <Card style={{width:300}} className="cardStyle">
        <CardActionArea>
          <CardMedia 
            image={item.image}
            title="Contemplative Reptile"
            style={{ width:'100%', height:200, backgroundSize:'contain' }}
          />
          <CardContent>
            <Typography gutterBottom variant="subtitle1" component="h4">
              {item.name}
            </Typography>
            <Typography variant="body2" color="textSecondary" component="p">
              {item.store}
            </Typography>
            <Typography variant="body2" color="textSecondary" component="p">
              {item.manufacturer}
            </Typography>
            <Typography variant="body2" color="textSecondary" component="p">
              {item.vikt}
            </Typography>
            <Divider />
            <Typography variant="h2" color="secondary" align="right">{item.price}:-
              {/* <Box bgcolor="secondary.main" color="primary.contrastText" style={{ padding:'6px', borderRadius:'10px', marginTop:'5px', width:'fit-content', marginLeft:'auto'}} >{item.price} kr</Box> */}
            </Typography>
          </CardContent>
        </CardActionArea>
        <CardActions disableSpacing>
        <IconButton aria-label="add to favorites">
          <Button variant="contained" color="secondary" size="small" startIcon={<ShoppingCart />}>
          Lägg i varukorg
          </Button>
        </IconButton>
        </CardActions>
      </Card>
      </Grid>
    );
  }

  renderCardGrid(fooditems: any[]){
    return (
      <Grid
        container
        spacing={4}
        className=""
        justify="center"
      >        
        {/* {this.singleCard()} */}
        {fooditems.map(item =>
        this.singleCard(item))}        
      </Grid>
    );
  }
  
  renderFoodTable(fooditems: any[]) {
    return (
      <table className='table table-striped' aria-labelledby="tableLabel">
        <thead>
          <th>ID</th>
          <th>Namn</th>
          <th>Pris</th>
          <th>Butik</th>
          <th>Märke</th>
          <th>Vikt</th>
        </thead>
        <tbody>
          {fooditems.map(item =>             
            this.renderItem(item)
          )}
        </tbody>
      </table>
    )
  }  

  handleRowClick(rowId:any) {
    const currentExpandedRows = this.state.expandedRows;
    const isRowCurrentlyExpanded = currentExpandedRows.includes(rowId);
    
    const newExpandedRows = isRowCurrentlyExpanded ? 
    currentExpandedRows.filter(id => id !== rowId) : 
    currentExpandedRows.concat(rowId);
    
    this.setState({expandedRows : newExpandedRows});
  }

  renderItemCaret(rowId:any) {
    const currentExpandedRows = this.state.expandedRows;
    const isRowCurrentlyExpanded = currentExpandedRows.includes(rowId);

    if (isRowCurrentlyExpanded) {
      return <Icon name="caret down" />;
    } else {
      return <Icon name="caret right" />;
    }
  }

  renderItem(item:any) {
    const clickCallback = () => this.handleRowClick(item.id);

    const itemRows = [
       <tr key={"row-data-" + item.id}>
              <td onClick={clickCallback} key={"row-data-" + item.id}>{item.id} {this.renderItemCaret(item.id)}</td>
              <td>{item.name}</td>
              <td>{item.price}</td>
              <td>{item.store}</td>
              <td>{item.manufacturer}</td>
              <td>{item.vikt}</td>
       </tr>
    ];

    if(this.state.expandedRows.includes(item.id)) {
      itemRows.push(
        <tr key={"row-expanded-" + item.id}>
                  <td>{item.name}</td>
                  <td><img src={item.image}></img></td>
                  <td>{item.manufacturer}</td>                  
                  {/* <td>{item.extraPrice.extrapris}</td> */}
                  
        </tr>
      );
    }

    return itemRows;
  }

  render() {
    //let contents = this.state.loading
    //  ? <p><em>Loading...</em></p>
      //: FetchData.renderForecastsTable(this.state.forecasts);
    //  :  this.renderFoodTable(this.state.foodItems);
      
      let cardView;
      if (!this.state.loading){
        cardView = this.renderCardGrid(this.state.foodItems);
      }

    return (
      <div>
        <h1 id="tabelLabel" >{this.props.children}</h1>
        <p>This component demonstrates fetching data from the server.</p>      
        {cardView}
      </div>
    );
  }

  async populateWeatherData() {
    const response = await fetch('weatherforecast');
    const data = await response.json();
    //this.setState({ forecasts: data, loading: false });
  }

  async populateFoodData() {
    const response = await fetch('api/fooditems/Meat');
    const data = await response.json();
    console.log(data);
    //this.setState({fooditems: data, loading: false});
    this.setState({foodItems: data, loading: false})
  }
  
}
