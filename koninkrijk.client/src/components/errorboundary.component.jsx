import React from 'react'

class ErrorBoundary extends React.Component {
    constructor(props) {
      super(props);
      this.state = { hasError: false, error: '', info: '', stack: '' }
    }
  
    static getDerivedStateFromError(error) {
      // Update state so the next render will show the fallback UI.
      return { hasError: true };
    }
  
    componentDidCatch(error, errorInfo) {
      // You can also log the error to an error reporting service
      // logErrorToMyService(error, errorInfo)
      this.setState({error, info: errorInfo, stack: error.stack})
    }
  
    render() {
      if (this.state.hasError) {
                  // You can render any custom fallback UI
        return (<div className="container d-flex align-items-center justify-content-center">
                <h4>Something went wrong!</h4>
                <br />
                <p><a href="/">Click here to return back to the main page</a></p>
                </div>);
      }
  
      return this.props.children; 
    }
}

export default ErrorBoundary;