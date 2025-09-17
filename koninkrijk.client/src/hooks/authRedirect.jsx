import { useEffect } from 'react';
import { useAuth } from '../services/AuthProvider';
import { useLocation } from "wouter";

const useAuthRedirect = () => {
  const [, setLocation] = useLocation();
  const { auth } = useAuth();

  useEffect(() => {
    if (!auth) {
      setLocation("/");
    }
  }, [auth, setLocation]);
};

export default useAuthRedirect;