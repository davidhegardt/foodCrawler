import React, { Component } from 'react';
interface IState {
  currentCount : number;
}

interface Props {

}

//export class Counter extends React.Component<IState,Props> {
export class Counter extends React.Component<Props,IState> {
  static displayName = Counter.name;
  readonly state = { currentCount: 0 };

  constructor(props: Props) {
    super(props);
    this.incrementCounter = this.incrementCounter.bind(this);
  }

  incrementCounter() {
    this.setState({
      currentCount: + 1
    });
  }

  render() {
    return (
      <div>
        <h1>Counter</h1>

        <p>This is a simple example of a React component.</p>

        <p aria-live="polite">Current count: <strong>{this.state.currentCount}</strong></p>

        <button className="btn btn-primary" onClick={this.incrementCounter}>Increment</button>
      </div>
    );
  }
}
