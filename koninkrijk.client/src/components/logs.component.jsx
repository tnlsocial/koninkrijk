/* eslint-disable react/prop-types */
import { Fragment, useEffect, useState } from 'react';
import apiService from '../services/api.service';
import useAuthRedirect from "../hooks/authRedirect";

const Logs = () => {
  useAuthRedirect();

  const [logs, setLogs] = useState([]);
  const [currentPage, setCurrentPage] = useState(0); // Start from page 0 to match the backend
  const [totalPages, setTotalPages] = useState(1);

  useEffect(() => {
    fetchLogs(currentPage);
  }, [currentPage]);

  const fetchLogs = (page) => {
    apiService.getLogsPaginated(page)
      .then((response) => {
        setLogs(response.logs);
        setTotalPages(response.totalPages);
      })
      .catch((error) => {
        console.error("An error occurred while fetching the logs:", error);
        setLogs([]);
      });
  };

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  return (
    <Fragment>
      {logs.length > 0 && (
        <div className="mx-auto col-6">
          <br />
          <table className='table'>
            <thead>
              <tr>
                <th>Datum</th>
                <th>Bericht</th>
              </tr>
            </thead>
            <tbody>
              {logs.map((log, index) => (
                <tr key={index}>
                  <td>{new Date(log.timestamp).toLocaleString()}</td>
                  <td>{log.message}</td>
                </tr>
              ))}
            </tbody>
          </table>
          <Pagination 
            currentPage={currentPage} 
            totalPages={totalPages} 
            onPageChange={handlePageChange} 
          />
        </div>
      )}
    </Fragment>
  );
};

const Pagination = ({ currentPage, totalPages, onPageChange }) => {
  const pages = [];

  for (let i = 0; i < totalPages; i++) { // Adjust to 0-based index
    pages.push(i);
  }

  return (
    <nav>
      <ul className="pagination justify-content-center">
        <li className={`page-item ${currentPage === 0 ? 'disabled' : ''}`}>
          <button className="page-link" onClick={() => onPageChange(currentPage - 1)} disabled={currentPage === 0}>
            Vorige
          </button>
        </li>
        {pages.map(page => (
          <li key={page} className={`page-item ${page === currentPage ? 'active' : ''}`}>
            <button className="page-link" onClick={() => onPageChange(page)}>
              {page + 1} {/* Display page numbers starting from 1 */}
            </button>
          </li>
        ))}
        <li className={`page-item ${currentPage === totalPages - 1 ? 'disabled' : ''}`}>
          <button className="page-link" onClick={() => onPageChange(currentPage + 1)} disabled={currentPage === totalPages - 1}>
            Volgende
          </button>
        </li>
      </ul>
    </nav>
  );
};

export default Logs;
