context('Authentication', () => {
  beforeEach(() => {
    const username = Cypress.env('username')
    const password = Cypress.env('password')

    if (typeof username !== 'string' || !username) {
      throw new Error('Missing username value')
    }
    if (typeof password !== 'string' || !password) {
      throw new Error('Missing password value')
    }

    cy.server()
    cy.route('POST', 'Login.aspx').as('login')
    cy.visit('Login.aspx')
  })

  it('can log in and out', () => {
    cy.get('[id$=txtUserId]').type(Cypress.env('username'))
    cy.get('[id$=txtPassword]').type(Cypress.env('password'), { log: false })
    cy.get('[id$=btnSignIn]').click()
    cy.wait('@login')

    cy.getCookie('ASP.NET_SessionId').should('exist')
    cy.get('h1').eq(0).should('have.text', 'GECO Home')
  })

  it('can not log in with bad password', () => {
    cy.get('[id$=txtUserId]').type(Cypress.env('username'))
    cy.get('[id$=txtPassword]').type('z')
    cy.get('[id$=btnSignIn]').click()
    cy.wait('@login')

    cy.getCookie('ASP.NET_SessionId').should('not.exist')
    cy.get('h1').should('not.exist')
    cy.get('[id$=lblMessage]').should('contain', 'the password is incorrect.')
  })

  it('can request password reset email', () => {
    cy.contains('Forgot password?').click()
    cy.get('[id$=txtEmailAddress]').type(Cypress.env('username'))
    cy.get('[id$=btnResetPassword]').click()
    cy.wait('@login')

    cy.get('[id$=Content_Panel1]').should(
      'contain.text',
      'If an account exists, then an email will be sent with instructions for resetting your password.'
    )
  })
})
