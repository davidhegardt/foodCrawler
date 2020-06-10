import React, { Component } from 'react';
import {Icon} from 'semantic-ui-react';


export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);        
    this.state = { fooditems: [], loading: true, expandedRows: [] };
  }

  componentDidMount() {
    //this.populateWeatherData();
    this.populateFoodData();
  }
  
  renderFoodTable(fooditems) {
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

  handleRowClick(rowId) {
    const currentExpandedRows = this.state.expandedRows;
    const isRowCurrentlyExpanded = currentExpandedRows.includes(rowId);
    
    const newExpandedRows = isRowCurrentlyExpanded ? 
    currentExpandedRows.filter(id => id !== rowId) : 
    currentExpandedRows.concat(rowId);
    
    this.setState({expandedRows : newExpandedRows});
  }

  renderItemCaret(rowId) {
    const currentExpandedRows = this.state.expandedRows;
    const isRowCurrentlyExpanded = currentExpandedRows.includes(rowId);

    if (isRowCurrentlyExpanded) {
      return <Icon name="caret down" />;
    } else {
      return <Icon name="caret right" />;
    }
  }

  renderItem(item) {
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
        </tr>
      );
    }

    return itemRows;
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      //: FetchData.renderForecastsTable(this.state.forecasts);
      : this.renderFoodTable(this.state.fooditems);
      

    return (
      <div>
        <h1 id="tabelLabel" >{this.props.match.params.id}</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

  async populateWeatherData() {
    const response = await fetch('weatherforecast');
    const data = await response.json();
    this.setState({ forecasts: data, loading: false });
  }

  async populateFoodData() {
    const response = await fetch('api/fooditems');
    const data = await response.json();
    console.log(data);
    this.setState({fooditems: data, loading: false});
  }
}
