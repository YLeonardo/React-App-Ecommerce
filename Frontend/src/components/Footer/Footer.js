import React from "react";

export const Footer = () => {
  const year = new Date().getFullYear();
  return (
    <>
      <footer><strong>{`T10-2019361171 ©${year}`}</strong></footer>
    </>
  );
};
